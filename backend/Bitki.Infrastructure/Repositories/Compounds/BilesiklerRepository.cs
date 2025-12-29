using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BilesiklerRepository : IBilesiklerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BilesiklerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bilesikler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description FROM dbo.bilesikler ORDER BY adi LIMIT 1000";
            return await connection.QueryAsync<Bilesikler>(sql);
        }

        public async Task<Bilesikler?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description FROM dbo.bilesikler WHERE bilesikid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Bilesikler>(sql, new { Id = id });
        }

        public async Task<long> AddAsync(Bilesikler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO dbo.bilesikler (adi, ingilizce, latince, aciklama) 
                        VALUES (@Name, @EnglishName, @LatinName, @Description) 
                        RETURNING bilesikid";
            return await connection.ExecuteScalarAsync<long>(sql, entity);
        }

        public async Task UpdateAsync(Bilesikler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"UPDATE dbo.bilesikler 
                        SET adi = @Name, ingilizce = @EnglishName, latince = @LatinName, aciklama = @Description 
                        WHERE bilesikid = @Id";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.bilesikler WHERE bilesikid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

