using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KullanimRepository : IKullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public KullanimRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Kullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level FROM dbo.kullanim ORDER BY kullanim";
            return await connection.QueryAsync<Kullanim>(sql);
        }
    }
}
