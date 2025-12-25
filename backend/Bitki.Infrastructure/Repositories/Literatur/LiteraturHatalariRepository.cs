using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturHatalariRepository : ILiteraturHatalariRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LiteraturHatalariRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<LiteraturHatalari>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, hataadi AS ErrorName, aciklama AS Description FROM dbo.literaturhatalari ORDER BY id LIMIT 1000";
            return await connection.QueryAsync<LiteraturHatalari>(sql);
        }
    }
}
