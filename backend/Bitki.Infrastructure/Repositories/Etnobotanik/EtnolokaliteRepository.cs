using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnolokaliteRepository : IEtnolokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EtnolokaliteRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Etnolokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnolokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content FROM dbo.etnolokalite LIMIT 1000");
        }

        public async Task<Etnolokalite?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnolokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content FROM dbo.etnolokalite WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnolokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnolokalite (yereladi, mevki, kullanim, icerik) VALUES (@LocalName, @Location, @Usage, @Content) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnolokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnolokalite SET yereladi = @LocalName, mevki = @Location, kullanim = @Usage, icerik = @Content WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnolokalite WHERE id = @Id", new { Id = id });
        }
    }
}
