using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class OtorRepository : IOtorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public OtorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Otor>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT otorid AS Id, otor AS Name, aciklama AS Description FROM dbo.otor ORDER BY otor LIMIT 1000";
            return await connection.QueryAsync<Otor>(sql);
        }

        public async Task<Otor?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Otor>(
                "SELECT otorid AS Id, otor AS Name, aciklama AS Description FROM dbo.otor WHERE otorid = @Id", new { Id = id });
        }

        public async Task<long> AddAsync(Otor entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<long>(
                "INSERT INTO dbo.otor (otor, aciklama) VALUES (@Name, @Description) RETURNING otorid", entity);
        }

        public async Task UpdateAsync(Otor entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.otor SET otor = @Name, aciklama = @Description WHERE otorid = @Id", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.otor WHERE otorid = @Id", new { Id = id });
        }
    }
}

