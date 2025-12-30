using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturRepository : ILiteraturRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public LiteraturRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // Define allowed and searchable columns
            var allowedColumns = new[] { "litid", "yazaradi", "arastirmaadi", "kaynakadi", "yili", "tamadi", "tip", "konutipi", "guvenilirlik" };
            var searchableColumns = new[] { "yazaradi", "arastirmaadi", "kaynakadi", "tamadi", "ozet" };

            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "litid" },
                { "AuthorName", "yazaradi" },
                { "ResearchName", "arastirmaadi" },
                { "SourceName", "kaynakadi" },
                { "Year", "yili" },
                { "FullName", "tamadi" },
                { "Type", "tip" },
                { "TopicType", "konutipi" },
                { "Reliability", "guvenilirlik" },
                { "Summary", "ozet" },
                { "Link", "link" }
            };

            _queryBuilder = new QueryBuilder("literatur", allowedColumns, searchableColumns, columnMappings);
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
                ORDER BY tamadi 
                LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Literatur>(sql);
        }

        public async Task<FilterResponse<Bitki.Core.Entities.Literatur>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // Build SELECT query
            var selectColumns = @"litid AS Id, yazaradi AS AuthorName, arastirmaadi AS ResearchName, 
                kaynakadi AS SourceName, yili AS Year, tamadi AS FullName, link AS Link, 
                tip AS Type, konutipi AS TopicType, guvenilirlik AS Reliability, ozet AS Summary";
            var selectSql = _queryBuilder.BuildSelectQuery(
                selectColumns,
                request.SearchText,
                request.Filters,
                request.SortColumn,
                request.SortDirection,
                parameters,
                request.IncludeDeleted,
                request.PageNumber,
                request.PageSize
            );

            // Build COUNT queries
            var totalCountSql = "SELECT COUNT(*) FROM dbo.literatur";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            // Execute queries
            var data = await connection.QueryAsync<Bitki.Core.Entities.Literatur>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Bitki.Core.Entities.Literatur>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Bitki.Core.Entities.Literatur?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"SELECT litid AS Id, yazaradi AS AuthorName, arastirmaadi AS ResearchName, 
                kaynakadi AS SourceName, yili AS Year, tamadi AS FullName, link AS Link, 
                tip AS Type, konutipi AS TopicType, guvenilirlik AS Reliability, ozet AS Summary 
                FROM dbo.literatur WHERE litid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Bitki.Core.Entities.Literatur>(sql, new { Id = id });
        }

        public async Task<long> AddAsync(Bitki.Core.Entities.Literatur entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO dbo.literatur (yazaradi, arastirmaadi, kaynakadi, yili, tamadi, link, tip, konutipi, guvenilirlik, ozet) 
                        VALUES (@AuthorName, @ResearchName, @SourceName, @Year, @FullName, @Link, @Type, @TopicType, @Reliability, @Summary) 
                        RETURNING litid";
            return await connection.ExecuteScalarAsync<long>(sql, entity);
        }

        public async Task UpdateAsync(Bitki.Core.Entities.Literatur entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"UPDATE dbo.literatur 
                        SET yazaradi = @AuthorName, arastirmaadi = @ResearchName, kaynakadi = @SourceName, 
                            yili = @Year, tamadi = @FullName, link = @Link, tip = @Type, 
                            konutipi = @TopicType, guvenilirlik = @Reliability, ozet = @Summary 
                        WHERE litid = @Id";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.literatur WHERE litid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
