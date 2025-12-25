using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BilesiklerRepository : IBilesiklerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BilesiklerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bilesikler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description FROM dbo.bilesikler ORDER BY adi LIMIT 1000";
            return await connection.QueryAsync<Bilesikler>(sql);
        }
    }
}
