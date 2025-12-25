using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteSaflastirmaRepository : IAktiviteSaflastirmaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteSaflastirmaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteSaflastirma>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktivitesaflastirma ORDER BY adi";
            return await connection.QueryAsync<AktiviteSaflastirma>(sql);
        }
    }
}
