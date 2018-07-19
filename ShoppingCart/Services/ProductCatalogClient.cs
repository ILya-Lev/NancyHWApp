using Newtonsoft.Json;
using Polly;
using ShoppingCart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        private static readonly string productCatalogBaseUrl = "http://localhost:44000";
        private static readonly string getProductPathTemplate = "/products?productids=[{0}]";

        //attempt variable is initialized from 1 to 3 inclusively!
        private static readonly Policy exponentialRetryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3,
                attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));


        //private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>()
        //{
        //    [1] = new Product { Id = 1, Name = "toothpaste", Price = 10.99m },
        //    [2] = new Product { Id = 2, Name = "mouthwash", Price = 23.49m },
        //};

        public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productIds)
        {
            return exponentialRetryPolicy.ExecuteAsync(async () =>
                await GetItemsFromCatalogueService(productIds).ConfigureAwait(false));

            //return productIds
            //    .Select(id => _products.ContainsKey(id) ? _products[id] : null)
            //    .Where(p => p != null);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogueService(int[] productIds)
        {
            var response = await RequestProductFromCatalogue(productIds);
            var items = await ConvertToShoppingCartItems(response);
            return items;
        }

        private async Task<HttpResponseMessage> RequestProductFromCatalogue(int[] productIds)
        {
            var requestAndPath = string.Format(getProductPathTemplate, string.Join(", ", productIds));
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(productCatalogBaseUrl);
                return await client.GetAsync(requestAndPath).ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            //throws HttpRequestException if response is not 2xx
            response.EnsureSuccessStatusCode();
            var productsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var products = JsonConvert.DeserializeObject<List<ProductCatalogProduct>>(productsString);
            return products.Select(p => new ShoppingCartItem(
                int.Parse(p.ProductId),
                p.ProductName,
                p.ProductDescription,
                p.Price
                ));
        }

        private class ProductCatalogProduct
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public Money Price { get; set; }
        }
    }
}