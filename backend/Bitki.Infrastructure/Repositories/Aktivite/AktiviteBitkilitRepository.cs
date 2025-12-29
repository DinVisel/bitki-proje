using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteBitkilitRepository : IAktiviteBitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteBitkilitRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteBitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteBitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId FROM dbo.aktivitebitkilit ORDER BY turkcead LIMIT 1000");
        }

        public async Task<AktiviteBitkilit?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteBitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId FROM dbo.aktivitebitkilit WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteBitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitebitkilit (turkcead, durum, litno, familyano, genusno) VALUES (@TurkishName, @Status, @LiteratureId, @FamilyId, @GenusId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteBitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitebitkilit SET turkcead = @TurkishName, durum = @Status, litno = @LiteratureId, familyano = @FamilyId, genusno = @GenusId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitebitkilit WHERE id = @Id", new { Id = id });
        }
    }
}
