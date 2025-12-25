using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteEtkiRepository : IAktiviteEtkiRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteEtkiRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteEtki>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktiviteetki ORDER BY adi";
            return await connection.QueryAsync<AktiviteEtki>(sql);
        }
    }
}
