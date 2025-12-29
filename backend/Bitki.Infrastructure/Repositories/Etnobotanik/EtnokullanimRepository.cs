using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnokullanimRepository : IEtnokullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EtnokullanimRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Etnokullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnokullanim>("SELECT id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate FROM dbo.etnokullanim LIMIT 1000");
        }

        public async Task<Etnokullanim?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnokullanim>("SELECT id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate FROM dbo.etnokullanim WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnokullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnokullanim (aciklama, lokaliteno, tariholusturma) VALUES (@Description, @LocalityId, @CreatedDate) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnokullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnokullanim SET aciklama = @Description, lokaliteno = @LocalityId, tariholusturma = @CreatedDate WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnokullanim WHERE id = @Id", new { Id = id });
        }
    }
}
