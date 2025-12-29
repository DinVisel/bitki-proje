using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class UcucuYagBilesikRepository : IUcucuYagBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public UcucuYagBilesikRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<UcucuYagBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<UcucuYagBilesik>("SELECT ucucuyagno AS EssentialOilId, bilesikno AS CompoundId, miktar AS Amount, birim AS Unit FROM dbo.ucucuyagbilesik LIMIT 1000");
        }

        public async Task<UcucuYagBilesik?> GetByIdAsync(int essentialOilId, int compoundId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UcucuYagBilesik>("SELECT ucucuyagno AS EssentialOilId, bilesikno AS CompoundId, miktar AS Amount, birim AS Unit FROM dbo.ucucuyagbilesik WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", new { EssentialOilId = essentialOilId, CompoundId = compoundId });
        }

        public async Task AddAsync(UcucuYagBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("INSERT INTO dbo.ucucuyagbilesik (ucucuyagno, bilesikno, miktar, birim) VALUES (@EssentialOilId, @CompoundId, @Amount, @Unit)", entity);
        }

        public async Task UpdateAsync(UcucuYagBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ucucuyagbilesik SET miktar = @Amount, birim = @Unit WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", entity);
        }

        public async Task DeleteAsync(int essentialOilId, int compoundId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ucucuyagbilesik WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", new { EssentialOilId = essentialOilId, CompoundId = compoundId });
        }
    }
}

