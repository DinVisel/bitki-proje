using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class OzellikRepository : IOzellikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public OzellikRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<Bitki.Core.Entities.Ozellik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Bitki.Core.Entities.Ozellik>("SELECT ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description FROM dbo.ozellik ORDER BY ozellikid LIMIT 1000");
        }

        public async Task<Bitki.Core.Entities.Ozellik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Bitki.Core.Entities.Ozellik>("SELECT ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description FROM dbo.ozellik WHERE ozellikid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Bitki.Core.Entities.Ozellik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ozellik (adi, tipno, aciklama) VALUES (@Name, @TypeId, @Description) RETURNING ozellikid", entity);
        }

        public async Task UpdateAsync(Bitki.Core.Entities.Ozellik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ozellik SET adi = @Name, tipno = @TypeId, aciklama = @Description WHERE ozellikid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ozellik WHERE ozellikid = @Id", new { Id = id });
        }
    }
}

