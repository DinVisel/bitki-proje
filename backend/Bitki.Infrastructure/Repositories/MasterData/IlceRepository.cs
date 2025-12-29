using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class IlceRepository : IIlceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public IlceRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Ilce>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT ilceid AS Id, ilce AS Name, sehirno AS CityId FROM dbo.ilce ORDER BY ilce LIMIT 1000";
            return await connection.QueryAsync<Ilce>(sql);
        }

        public async Task<Ilce?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Ilce>("SELECT ilceid AS Id, ilce AS Name, sehirno AS CityId FROM dbo.ilce WHERE ilceid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Ilce entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ilce (ilce, sehirno) VALUES (@Name, @CityId) RETURNING ilceid", entity);
        }

        public async Task UpdateAsync(Ilce entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ilce SET ilce = @Name, sehirno = @CityId WHERE ilceid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ilce WHERE ilceid = @Id", new { Id = id });
        }
    }
}

