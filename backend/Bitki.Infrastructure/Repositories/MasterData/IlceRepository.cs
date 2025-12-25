using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class IlceRepository : IIlceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public IlceRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Ilce>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT ilceid AS Id, ilce AS Name, sehirno AS CityId FROM dbo.ilce ORDER BY ilce LIMIT 1000";
            return await connection.QueryAsync<Ilce>(sql);
        }
    }
}
