using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class UlkeRepository : IUlkeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UlkeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Ulke>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ulke AS Name FROM dbo.ulke ORDER BY ulke";
            return await connection.QueryAsync<Ulke>(sql);
        }

        public async Task<Ulke?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Ulke>("SELECT id AS Id, ulke AS Name FROM dbo.ulke WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Ulke entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ulke (ulke) VALUES (@Name) RETURNING id", entity);
        }

        public async Task UpdateAsync(Ulke entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ulke SET ulke = @Name WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ulke WHERE id = @Id", new { Id = id });
        }
    }
}

