using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class SehirRepository : ISehirRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SehirRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Sehir>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode FROM dbo.sehir ORDER BY sehir";
            return await connection.QueryAsync<Sehir>(sql);
        }

        public async Task<Sehir?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Sehir>("SELECT sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode FROM dbo.sehir WHERE sehirid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Sehir entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.sehir (sehir, trafikkodu) VALUES (@Name, @TrafficCode) RETURNING sehirid", entity);
        }

        public async Task UpdateAsync(Sehir entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.sehir SET sehir = @Name, trafikkodu = @TrafficCode WHERE sehirid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.sehir WHERE sehirid = @Id", new { Id = id });
        }
    }
}

