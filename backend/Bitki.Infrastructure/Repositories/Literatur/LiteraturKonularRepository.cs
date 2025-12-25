using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturKonularRepository : ILiteraturKonularRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LiteraturKonularRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<LiteraturKonular>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, konu AS Topic, aciklama AS Description FROM dbo.literaturkonular ORDER BY id LIMIT 1000";
            return await connection.QueryAsync<LiteraturKonular>(sql);
        }
    }
}
