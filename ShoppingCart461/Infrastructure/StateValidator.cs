using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ShoppingCart461.Infrastructure
{
    class StateValidator : IStateValidator
    {
        private const string _connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\programming\projects\aspnet\NancyHWApp\databases\ShoppingCart_db.mdf;Integrated Security=True;Connect Timeout=30;Initial Catalog=ShoppingCart";

        private const string _getCartsAmountSql = @"SELECT count(Id) FROM ShoppingCart";
        private readonly int _cartsAmountThreshold;

        public StateValidator(int cartsAmountThreshold)
        {
            _cartsAmountThreshold = cartsAmountThreshold;
        }

        public async Task<bool> IsValidState()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cartsAmount = (int)(await connection.QueryAsync(_getCartsAmountSql)).Single();
                return cartsAmount > _cartsAmountThreshold;
            }
        }
    }
}