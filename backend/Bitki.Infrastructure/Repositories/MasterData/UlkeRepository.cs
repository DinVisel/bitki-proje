using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class UlkeRepository : IUlkeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UlkeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Ulke>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ulke AS Name FROM dbo.ulke ORDER BY ulke";
            return await connection.QueryAsync<Ulke>(sql);
        }
    }
}
