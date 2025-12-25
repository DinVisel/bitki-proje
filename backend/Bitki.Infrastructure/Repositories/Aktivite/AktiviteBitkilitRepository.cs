using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteBitkilitRepository : IAktiviteBitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteBitkilitRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteBitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId FROM dbo.aktivitebitkilit ORDER BY turkcead LIMIT 1000";
            return await connection.QueryAsync<AktiviteBitkilit>(sql);
        }
    }
}
