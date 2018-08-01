using Dapper;
using Newtonsoft.Json;
using ShoppingCart.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class DbEventStore : IEventStore
    {
        private const string _connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\programming\projects\aspnet\NancyHWApp\databases\ShoppingCart_db.mdf;Integrated Security=True;Connect Timeout=30;Initial Catalog=ShoppingCart";

        private const string _addEventSql =
            @"insert into EventStore (Name, OccuredAt, Content)
                values (@Name, @OccuredAt, @Content)";

        private const string _readEventsSql =
            @"select *
                from EventStore
                where id >= @start and id <= @end";

        public async Task Raise(string eventName, object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(_addEventSql, new
                {
                    Name = eventName,
                    OccuredAt = DateTime.Now,
                    Content = jsonContent
                });
            }
        }

        public async Task<IEnumerable<Event>> GetEvents(int first, int last)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var events = (await connection.QueryAsync<dynamic>(_readEventsSql, new
                {
                    Start = first,
                    End = last
                }).ConfigureAwait(false))
                .Select(row =>
                {
                    var content = JsonConvert.DeserializeObject(row.Content);
                    return new Event(row.Id, row.OccuredAt, row.Name, content);
                });
                return events;
            }
        }
    }
}