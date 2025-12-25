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

        public UcucuYagBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UcucuYagBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT ucucuyagno AS EssentialOilId, bilesikno AS CompoundId, miktar AS Amount, birim AS Unit FROM dbo.ucucuyagbilesik LIMIT 1000";
            return await connection.QueryAsync<UcucuYagBilesik>(sql);
        }
    }
}
