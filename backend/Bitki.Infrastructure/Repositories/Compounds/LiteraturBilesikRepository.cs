using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class LiteraturBilesikRepository : ILiteraturBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LiteraturBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<LiteraturBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, literaturno AS LiteratureId, bilesikno AS CompoundId, aciklama AS Description FROM dbo.literaturbilesik ORDER BY id DESC LIMIT 1000";
            return await connection.QueryAsync<LiteraturBilesik>(sql);
        }
    }
}
