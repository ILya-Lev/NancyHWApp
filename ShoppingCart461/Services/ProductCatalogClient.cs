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
        private readonly ICache _cache;
        private static readonly string productCatalogBaseUrl = "http://localhost:18169";
        private static readonly string getProductPathTemplate = "/products?productids={0}";

        //attempt variable is initialized from 1 to 3 inclusively!
        private static readonly Policy exponentialRetryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3,
                attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));


        private readonly Dictionary<int, ShoppingCartItem> _products = new Dictionary<int, ShoppingCartItem>()
        {
            [1] = new ShoppingCartItem(1, "toothpaste", "", new Money { Amount = 10.99m }),
            [2] = new ShoppingCartItem(2, "mouthwash", "", new Money { Amount = 23.49m }),
        };

        public ProductCatalogClient(ICache cache)
        {
            _cache = cache;
        }

        public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productIds)
        {
            return exponentialRetryPolicy.ExecuteAsync(async () =>
                await GetItemsFromCatalogueService(productIds).ConfigureAwait(false));

            //var cartItems = productIds
            //    .Select(id => _products.ContainsKey(id) ? _products[id] : null)
            //    .Where(p => p != null);
            //return Task.FromResult(cartItems);
        }

        private async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogueService(int[] productIds)
        {
            var response = await RequestProductFromCatalogue(productIds);
            var items = await ConvertToShoppingCartItems(response);
            return items;
        }

        private async Task<HttpResponseMessage> RequestProductFromCatalogue(int[] productIds)
        {
            var requestAndPath = string.Format(getProductPathTemplate, string.Join(", ", productIds));
            var response = _cache.Get(requestAndPath) as HttpResponseMessage;

            if (response == null)
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(productCatalogBaseUrl);
                    response = await client.GetAsync(requestAndPath).ConfigureAwait(false);

                    AddToCache(requestAndPath, response);
                }

            return response;
        }

        private void AddToCache(string key, HttpResponseMessage response)
        {
            var maxAge = response.Headers.CacheControl?.MaxAge;
            if (maxAge.HasValue)
                _cache.Add(key, response, maxAge.Value);
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