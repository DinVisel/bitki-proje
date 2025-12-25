using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class SehirRepository : ISehirRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SehirRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Sehir>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode FROM dbo.sehir ORDER BY sehir";
            return await connection.QueryAsync<Sehir>(sql);
        }
    }
}
