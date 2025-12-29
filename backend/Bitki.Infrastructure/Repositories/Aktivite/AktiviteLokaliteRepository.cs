using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteLokaliteRepository : IAktiviteLokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AktiviteLokaliteRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<AktiviteLokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteLokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId FROM dbo.aktivitelokalite ORDER BY yereladi LIMIT 1000");
        }

        public async Task<AktiviteLokalite?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteLokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId FROM dbo.aktivitelokalite WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteLokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitelokalite (yereladi, mevki, sehirno, ilceno) VALUES (@LocalName, @Location, @CityId, @DistrictId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteLokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitelokalite SET yereladi = @LocalName, mevki = @Location, sehirno = @CityId, ilceno = @DistrictId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitelokalite WHERE id = @Id", new { Id = id });
        }
    }
}
