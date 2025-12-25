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
            _queryBuilder = new QueryBuilder("literatur", allowedColumns, searchableColumns);
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

        public async Task<FilterResponse<Bitki.Core.Entities.Literatur>> QueryAsync(FilterRequest request)
        {
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
                request.IncludeDeleted
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
    }
}
