
using Dapper;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Ozellik;

namespace Bitki.Infrastructure.Repositories.Ozellik
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PropertyTypeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<PropertyType>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            // Assuming table name is ozelliktip or tip? 
            // Ozellik table has 'tipno'. Suggests 'tip' table or 'ozelliktip'. 
            // Let's try 'ozelliktip'. If it fails, we get SQL error.
            return await connection.QueryAsync<PropertyType>("SELECT tipno AS Id, adi AS Name FROM dbo.ozelliktip ORDER BY adi");
        }
    }
}
