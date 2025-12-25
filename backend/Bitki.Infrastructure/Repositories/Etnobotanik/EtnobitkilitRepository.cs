using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnobitkilitRepository : IEtnobitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EtnobitkilitRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Etnobitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId FROM dbo.etnobitkilit ORDER BY turkcead LIMIT 1000";
            return await connection.QueryAsync<Etnobitkilit>(sql);
        }
    }
}
