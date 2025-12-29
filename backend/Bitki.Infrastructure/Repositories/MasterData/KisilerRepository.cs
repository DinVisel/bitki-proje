using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KisilerRepository : IKisilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public KisilerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Kisiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName FROM dbo.kisiler ORDER BY isim, soyisim LIMIT 1000";
            return await connection.QueryAsync<Kisiler>(sql);
        }

        public async Task<Kisiler?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Kisiler>("SELECT kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName FROM dbo.kisiler WHERE kisiid = @Id", new { Id = id });
        }

        public async Task<long> AddAsync(Kisiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<long>("INSERT INTO dbo.kisiler (adi, isim, soyisim) VALUES (@FullName, @FirstName, @LastName) RETURNING kisiid", entity);
        }

        public async Task UpdateAsync(Kisiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.kisiler SET adi = @FullName, isim = @FirstName, soyisim = @LastName WHERE kisiid = @Id", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.kisiler WHERE kisiid = @Id", new { Id = id });
        }
    }
}

