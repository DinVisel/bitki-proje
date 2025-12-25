using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteTestYeriRepository : IAktiviteTestYeriRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteTestYeriRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteTestYeri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktivitetestyeri ORDER BY adi";
            return await connection.QueryAsync<AktiviteTestYeri>(sql);
        }
    }
}
