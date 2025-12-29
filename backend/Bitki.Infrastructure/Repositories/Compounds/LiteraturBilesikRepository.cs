using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class LiteraturBilesikRepository : ILiteraturBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public LiteraturBilesikRepository(IDbConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        public async Task<IEnumerable<LiteraturBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturBilesik>("SELECT id AS Id, literaturno AS LiteratureId, bilesikno AS CompoundId, aciklama AS Description FROM dbo.literaturbilesik ORDER BY id DESC LIMIT 1000");
        }

        public async Task<LiteraturBilesik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturBilesik>("SELECT id AS Id, literaturno AS LiteratureId, bilesikno AS CompoundId, aciklama AS Description FROM dbo.literaturbilesik WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturbilesik (literaturno, bilesikno, aciklama) VALUES (@LiteratureId, @CompoundId, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturbilesik SET literaturno = @LiteratureId, bilesikno = @CompoundId, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturbilesik WHERE id = @Id", new { Id = id });
        }
    }
}
