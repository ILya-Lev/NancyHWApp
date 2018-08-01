using Dapper;
using ShoppingCart.Model;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string _connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\programming\projects\aspnet\NancyHWApp\databases\ShoppingCart_db.mdf;Integrated Security=True;Connect Timeout=30;Initial Catalog=ShoppingCart";

        private const string _readItemsSql =
            @"select sci.ProductCatalogId, sci.ProductName, sci.ProductDescription, sci.Amount, sci.Currency
                FROM ShoppingCart sc inner join ShoppingCartItem sci on sc.[Id] = sci.[ShoppingCartId]
                where sc.UserId=@UserId";

        private const string _clearCartSql =
            @"delete sci.*
                from ShoppingCartItem sci inner join ShoppingCart sc on sci.[ShoppingCartId] = sc.[Id]
                where sc.UserId=@UserId";

        private const string _fillInCartSql =
            @"insert into ShoppingCartItem ([ShoppingCartId], [ProductCatalogId], [ProductName], [ProductDescription], [Amount], [Currency])
              values (@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

        public async Task<ShoppingCartModel> Get(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var items = await connection.QueryAsync(_readItemsSql, new { UserId = userId })
                                    .ConfigureAwait(false);

                    var shoppingCartItems = items
                        .Select(i => new ShoppingCartItem((int)i.ProductCatalogId, i.ProductName, i.ProductDescription,
                        new Money { Amount = i.Amount, Currency = i.Currency }));

                    return new ShoppingCartModel
                    {
                        UserId = userId,
                        Items = shoppingCartItems
                    };
                }
                catch (AggregateException aexc)
                {
                    Debug.Print(aexc.Message);
                }
                catch (Exception exc)
                {
                    Debug.Print(exc.Message);
                }
            }

            return null;
        }

        public async Task Save(ShoppingCartModel shoppingCart)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var transaction = connection.BeginTransaction("Saving items into Cart"))
            {
                await connection.ExecuteAsync(_clearCartSql, new { shoppingCart.UserId }, transaction)
                    .ConfigureAwait(false);

                await connection.ExecuteAsync(_fillInCartSql, shoppingCart.Items, transaction);
            }
        }
    }
}