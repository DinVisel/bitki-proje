using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class EtkilerRepository : IEtkilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EtkilerRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Etkiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etkiler>("SELECT etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description FROM dbo.etkiler ORDER BY etkiid LIMIT 1000");
        }

        public async Task<Etkiler?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etkiler>("SELECT etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description FROM dbo.etkiler WHERE etkiid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etkiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etkiler (adi, latince, ingilizce, aciklama) VALUES (@Name, @LatinName, @EnglishName, @Description) RETURNING etkiid", entity);
        }

        public async Task UpdateAsync(Etkiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etkiler SET adi = @Name, latince = @LatinName, ingilizce = @EnglishName, aciklama = @Description WHERE etkiid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etkiler WHERE etkiid = @Id", new { Id = id });
        }
    }
}

