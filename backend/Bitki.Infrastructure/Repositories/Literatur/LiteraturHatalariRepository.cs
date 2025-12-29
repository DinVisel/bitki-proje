using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturHatalariRepository : ILiteraturHatalariRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public LiteraturHatalariRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<LiteraturHatalari>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturHatalari>("SELECT id AS Id, hataadi AS ErrorName, aciklama AS Description FROM dbo.literaturhatalari ORDER BY id LIMIT 1000");
        }

        public async Task<LiteraturHatalari?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturHatalari>("SELECT id AS Id, hataadi AS ErrorName, aciklama AS Description FROM dbo.literaturhatalari WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturHatalari entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturhatalari (hataadi, aciklama) VALUES (@ErrorName, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturHatalari entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturhatalari SET hataadi = @ErrorName, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturhatalari WHERE id = @Id", new { Id = id });
        }
    }
}

