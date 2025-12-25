using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class BitkiResimleriRepository : IBitkiResimleriRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BitkiResimleriRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<BitkiResimleri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri ORDER BY bitkiresimid LIMIT 1000";
            return await connection.QueryAsync<BitkiResimleri>(sql);
        }

        public async Task<IEnumerable<BitkiResimleri>> GetByPlantIdAsync(int plantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri WHERE bitkino = @PlantId ORDER BY bitkiresimid";
            return await connection.QueryAsync<BitkiResimleri>(sql, new { PlantId = plantId });
        }
    }
}
