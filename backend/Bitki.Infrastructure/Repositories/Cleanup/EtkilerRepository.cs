using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class EtkilerRepository : IEtkilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EtkilerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Etkiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description FROM dbo.etkiler ORDER BY etkiid LIMIT 1000";
            return await connection.QueryAsync<Etkiler>(sql);
        }
    }
}
