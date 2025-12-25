using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class UcuyagRepository : IUcuyagRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UcuyagRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ucucuyagadi AS Name, yereladi AS LocalName, kullanim AS Usage FROM dbo.ucuyag ORDER BY id LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Ucuyag>(sql);
        }
    }
}
