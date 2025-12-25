using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories;
using Dapper;

namespace Bitki.Infrastructure.Repositories
{
    public class BitkiRepository : IBitkiRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BitkiRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Plant>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT 
                    bitkiid AS Id, 
                    turkce AS Name, 
                    bitki AS LatinName, 
                    aciklama AS Description
                FROM dbo.bitki";
            return await connection.QueryAsync<Plant>(sql);
        }

        public async Task<Plant?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT 
                    bitkiid AS Id, 
                    turkce AS Name, 
                    bitki AS LatinName, 
                    aciklama AS Description
                FROM dbo.bitki 
                WHERE bitkiid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Plant>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Plant plant)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                INSERT INTO dbo.bitki (turkce, bitki, aciklama, tibbi, gida, kultur, zehir, tf, adalar, varliksupheli, revizyon, ex, eksikteshis, kontrolok, yayinok) 
                VALUES (@Name, @LatinName, @Description, false, false, false, false, false, false, false, false, false, false, false, false)
                RETURNING bitkiid";
            return await connection.ExecuteScalarAsync<int>(sql, plant);
        }

        public async Task UpdateAsync(Plant plant)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                UPDATE dbo.bitki 
                SET turkce = @Name, 
                    bitki = @LatinName, 
                    aciklama = @Description
                WHERE bitkiid = @Id";
            await connection.ExecuteAsync(sql, plant);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.bitki WHERE bitkiid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
