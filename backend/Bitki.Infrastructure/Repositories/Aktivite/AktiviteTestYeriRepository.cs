using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteTestYeriRepository : IAktiviteTestYeriRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteTestYeriRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteTestYeri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteTestYeri>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktivitetestyeri ORDER BY adi");
        }

        public async Task<AktiviteTestYeri?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteTestYeri>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktivitetestyeri WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteTestYeri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitetestyeri (adi, aciklama) VALUES (@Name, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteTestYeri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitetestyeri SET adi = @Name, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitetestyeri WHERE id = @Id", new { Id = id });
        }
    }
}
