using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KullanimRepository : IKullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public KullanimRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Kullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Kullanim>("SELECT id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level FROM dbo.kullanim ORDER BY kullanim");
        }

        public async Task<Kullanim?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Kullanim>("SELECT id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level FROM dbo.kullanim WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Kullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.kullanim (kullanim, tip, seviye) VALUES (@UsageName, @Type, @Level) RETURNING id", entity);
        }

        public async Task UpdateAsync(Kullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.kullanim SET kullanim = @UsageName, tip = @Type, seviye = @Level WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.kullanim WHERE id = @Id", new { Id = id });
        }
    }
}

