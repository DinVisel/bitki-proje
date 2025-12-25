using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Taxonomy;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Taxonomy
{
    public class GenusRepository : IGenusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenusRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Genus>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT genusid AS Id, genus AS Name, familyano AS FamilyId, aciklama AS Description FROM dbo.genus ORDER BY genus LIMIT 1000";
            return await connection.QueryAsync<Genus>(sql);
        }
    }
}
