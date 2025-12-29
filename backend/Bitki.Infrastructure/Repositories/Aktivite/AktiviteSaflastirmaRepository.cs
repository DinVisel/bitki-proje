using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteSaflastirmaRepository : IAktiviteSaflastirmaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteSaflastirmaRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteSaflastirma>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteSaflastirma>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktivitesaflastirma ORDER BY adi");
        }

        public async Task<AktiviteSaflastirma?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteSaflastirma>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktivitesaflastirma WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteSaflastirma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitesaflastirma (adi, ingilizce, aciklama) VALUES (@Name, @EnglishName, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteSaflastirma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitesaflastirma SET adi = @Name, ingilizce = @EnglishName, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitesaflastirma WHERE id = @Id", new { Id = id });
        }
    }
}
