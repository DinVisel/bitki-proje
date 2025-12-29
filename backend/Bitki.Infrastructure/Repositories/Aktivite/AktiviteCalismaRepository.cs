using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteCalismaRepository : IAktiviteCalismaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteCalismaRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteCalisma>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteCalisma>("SELECT id AS Id, aciklama AS Description, tariholusturma AS CreatedDate, lokaliteno AS LocalityId, etkino AS EffectId FROM dbo.aktivitecalisma ORDER BY id DESC LIMIT 1000");
        }

        public async Task<AktiviteCalisma?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteCalisma>("SELECT id AS Id, aciklama AS Description, tariholusturma AS CreatedDate, lokaliteno AS LocalityId, etkino AS EffectId FROM dbo.aktivitecalisma WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteCalisma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitecalisma (aciklama, tariholusturma, lokaliteno, etkino) VALUES (@Description, @CreatedDate, @LocalityId, @EffectId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteCalisma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitecalisma SET aciklama = @Description, tariholusturma = @CreatedDate, lokaliteno = @LocalityId, etkino = @EffectId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitecalisma WHERE id = @Id", new { Id = id });
        }
    }
}
