using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturKonularRepository : ILiteraturKonularRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public LiteraturKonularRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<LiteraturKonular>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturKonular>("SELECT id AS Id, konu AS Topic, aciklama AS Description FROM dbo.literaturkonular ORDER BY id LIMIT 1000");
        }

        public async Task<LiteraturKonular?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturKonular>("SELECT id AS Id, konu AS Topic, aciklama AS Description FROM dbo.literaturkonular WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturKonular entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturkonular (konu, aciklama) VALUES (@Topic, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturKonular entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturkonular SET konu = @Topic, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturkonular WHERE id = @Id", new { Id = id });
        }
    }
}

