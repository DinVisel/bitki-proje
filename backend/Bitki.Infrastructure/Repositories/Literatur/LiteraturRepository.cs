using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturRepository : ILiteraturRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LiteraturRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Literatur>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT 
                    litid AS Id, 
                    yazaradi AS AuthorName, 
                    arastirmaadi AS ResearchName, 
                    kaynakadi AS SourceName, 
                    yili AS Year, 
                    tamadi AS FullName, 
                    link AS Link, 
                    tip AS Type, 
                    konutipi AS TopicType, 
                    guvenilirlik AS Reliability, 
                    ozet AS Summary 
                FROM dbo.literatur 
                ORDER BY litid DESC 
                LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Literatur>(sql);
        }
    }
}
