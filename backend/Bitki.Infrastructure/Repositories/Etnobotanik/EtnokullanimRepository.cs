using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnokullanimRepository : IEtnokullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EtnokullanimRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Etnokullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate FROM dbo.etnokullanim LIMIT 1000";
            return await connection.QueryAsync<Etnokullanim>(sql);
        }
    }
}
