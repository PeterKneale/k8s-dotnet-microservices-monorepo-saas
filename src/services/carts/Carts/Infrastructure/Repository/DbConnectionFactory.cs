using System.Data;
using Npgsql;

namespace Carts.Infrastructure.Repository
{

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        
        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
    
}