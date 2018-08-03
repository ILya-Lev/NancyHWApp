using Nancy;
using ProductCatalog.Model;
using ProductCatalog.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog
{
    public class ProductsModule : NancyModule
    {
        public ProductsModule(IProductsStore productsStore) : base("/products")
        {
            Get("", _ =>
            {
                var productsString = Request.Query.productIds;
                var productIds = ParseProductIdsString(productsString);
                IEnumerable<Product> products = productsStore.GetProductsByIds(productIds);

                //todo: how to add e-tag?
                return Negotiate
                    .WithHeader("cache-control", "private; max-age: 3600")
                    .WithModel(products);
            });
        }

        private int[] ParseProductIdsString(string productsString)
        {
            if (string.IsNullOrWhiteSpace(productsString))
                return new int[0];

            return productsString.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(slice => new
                {
                    Success = int.TryParse(slice, out int value),
                    Id = value
                })
                .Where(parseMetadata => parseMetadata.Success)
                .Select(parseMetadata => parseMetadata.Id)
                .ToArray();
        }
    }
}
