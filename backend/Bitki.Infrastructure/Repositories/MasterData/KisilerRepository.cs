using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KisilerRepository : IKisilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public KisilerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Kisiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName FROM dbo.kisiler ORDER BY isim, soyisim LIMIT 1000";
            return await connection.QueryAsync<Kisiler>(sql);
        }
    }
}
