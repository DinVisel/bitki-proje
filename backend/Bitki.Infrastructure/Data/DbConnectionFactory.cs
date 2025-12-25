using System.Data;
using Bitki.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Bitki.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        private readonly NpgsqlDataSource _dataSource;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            // Use NpgsqlDataSource for proper connection pooling
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            _dataSource = dataSourceBuilder.Build();
        }

        public IDbConnection CreateConnection()
        {
            return _dataSource.CreateConnection();
        }
    }
}
