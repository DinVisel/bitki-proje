using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteEtkiRepository : IAktiviteEtkiRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteEtkiRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteEtki>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteEtki>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktiviteetki ORDER BY adi");
        }

        public async Task<AktiviteEtki?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteEtki>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktiviteetki WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteEtki entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktiviteetki (adi, ingilizce, aciklama) VALUES (@Name, @EnglishName, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteEtki entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktiviteetki SET adi = @Name, ingilizce = @EnglishName, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktiviteetki WHERE id = @Id", new { Id = id });
        }
    }
}
