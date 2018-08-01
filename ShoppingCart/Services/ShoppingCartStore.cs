using Dapper;
using ShoppingCart.Model;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string _connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\programming\projects\aspnet\NancyHWApp\databases\ShoppingCart_db.mdf;Integrated Security=True;Connect Timeout=30;Initial Catalog=ShoppingCart";

        private const string _readItemsSql =
            @"select * 
                from ShoppingCart sc inner join ShoppingCartItem sci on sc.[Id] = sci.[ShoppingCartId]
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
                var items = await connection.QueryAsync<ShoppingCartItem>(_readItemsSql, new { UserId = userId });

                return new ShoppingCartModel
                {
                    UserId = userId,
                    Items = items
                };
            }
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