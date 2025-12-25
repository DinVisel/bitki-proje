using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class OzellikRepository : IOzellikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public OzellikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Ozellik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description FROM dbo.ozellik ORDER BY ozellikid LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Ozellik>(sql);
        }
    }
}
