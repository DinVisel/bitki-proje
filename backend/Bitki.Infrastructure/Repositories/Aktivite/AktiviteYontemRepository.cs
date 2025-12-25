using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteYontemRepository : IAktiviteYontemRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteYontemRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteYontem>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktiviteyontem ORDER BY adi";
            return await connection.QueryAsync<AktiviteYontem>(sql);
        }
    }
}
