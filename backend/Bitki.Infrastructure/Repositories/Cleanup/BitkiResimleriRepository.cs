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
        public BitkiResimleriRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<BitkiResimleri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri ORDER BY bitkiresimid LIMIT 1000");
        }

        public async Task<IEnumerable<BitkiResimleri>> GetByPlantIdAsync(int plantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri WHERE bitkino = @PlantId ORDER BY bitkiresimid", new { PlantId = plantId });
        }

        public async Task<BitkiResimleri?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri WHERE bitkiresimid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(BitkiResimleri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.bitkiresimleri (bitkino, resimyeri, aciklama) VALUES (@PlantId, @ImageLocation, @Description) RETURNING bitkiresimid", entity);
        }

        public async Task UpdateAsync(BitkiResimleri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.bitkiresimleri SET bitkino = @PlantId, resimyeri = @ImageLocation, aciklama = @Description WHERE bitkiresimid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.bitkiresimleri WHERE bitkiresimid = @Id", new { Id = id });
        }
    }
}

