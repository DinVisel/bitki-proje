using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteCalismaRepository : IAktiviteCalismaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteCalismaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteCalisma>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, aciklama AS Description, tariholusturma AS CreatedDate, lokaliteno AS LocalityId, etkino AS EffectId FROM dbo.aktivitecalisma ORDER BY id DESC LIMIT 1000";
            return await connection.QueryAsync<AktiviteCalisma>(sql);
        }
    }
}
