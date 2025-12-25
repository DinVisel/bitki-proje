using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class OtorRepository : IOtorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public OtorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Otor>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT otorid AS Id, otor AS Name, aciklama AS Description FROM dbo.otor ORDER BY otor LIMIT 1000";
            return await connection.QueryAsync<Otor>(sql);
        }
    }
}
