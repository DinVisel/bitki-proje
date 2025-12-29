using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteYontemRepository : IAktiviteYontemRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteYontemRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteYontem>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteYontem>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktiviteyontem ORDER BY adi");
        }

        public async Task<AktiviteYontem?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteYontem>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktiviteyontem WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteYontem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktiviteyontem (adi, aciklama) VALUES (@Name, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteYontem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktiviteyontem SET adi = @Name, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktiviteyontem WHERE id = @Id", new { Id = id });
        }
    }
}
