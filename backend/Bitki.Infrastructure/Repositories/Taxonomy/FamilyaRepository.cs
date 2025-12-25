using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Taxonomy;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Taxonomy
{
    public class FamilyaRepository : IFamilyaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public FamilyaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Familya>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT familyaid AS Id, familya AS Name, turkce AS TurkishName FROM dbo.familya ORDER BY familya LIMIT 1000";
            return await connection.QueryAsync<Familya>(sql);
        }
    }
}
