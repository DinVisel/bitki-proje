using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnobitkilitRepository : IEtnobitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EtnobitkilitRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Etnobitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnobitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId FROM dbo.etnobitkilit ORDER BY turkcead LIMIT 1000");
        }

        public async Task<Etnobitkilit?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnobitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId FROM dbo.etnobitkilit WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnobitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnobitkilit (turkcead, durum, litno) VALUES (@TurkishName, @Status, @LiteratureId) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnobitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnobitkilit SET turkcead = @TurkishName, durum = @Status, litno = @LiteratureId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnobitkilit WHERE id = @Id", new { Id = id });
        }
    }
}
