using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BitkiBilesikRepository : IBitkiBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public BitkiBilesikRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<BitkiBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiBilesik>("SELECT id AS Id, bitkino AS PlantId, bilesikno AS CompoundId, miktar AS Amount, aciklama AS Description FROM dbo.bitkibilesik ORDER BY id DESC LIMIT 1000");
        }

        public async Task<BitkiBilesik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BitkiBilesik>("SELECT id AS Id, bitkino AS PlantId, bilesikno AS CompoundId, miktar AS Amount, aciklama AS Description FROM dbo.bitkibilesik WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(BitkiBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.bitkibilesik (bitkino, bilesikno, miktar, aciklama) VALUES (@PlantId, @CompoundId, @Amount, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(BitkiBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.bitkibilesik SET bitkino = @PlantId, bilesikno = @CompoundId, miktar = @Amount, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.bitkibilesik WHERE id = @Id", new { Id = id });
        }
    }
}

