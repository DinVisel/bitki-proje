using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class UcuyagRepository : IUcuyagRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public UcuyagRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // Define allowed and searchable columns
            var allowedColumns = new[] { "id", "ucucuyagadi", "yereladi", "kullanim" };
            var searchableColumns = new[] { "ucucuyagadi", "yereladi", "kullanim" };
            _queryBuilder = new QueryBuilder("ucuyag", allowedColumns, searchableColumns);
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ucucuyagadi AS Name, yereladi AS LocalName, kullanim AS Usage FROM dbo.ucuyag ORDER BY id LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Ucuyag>(sql);
        }

        public async Task<FilterResponse<Bitki.Core.Entities.Ucuyag>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination(); // Validate pagination parameters

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // Build SELECT query
            var selectColumns = "id AS Id, ucucuyagadi AS Name, yereladi AS LocalName, kullanim AS Usage";
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

            // Build COUNT query for total records
            var totalCountSql = "SELECT COUNT(*) FROM dbo.ucuyag";

            // Build COUNT query for filtered records
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            // Execute queries
            var data = await connection.QueryAsync<Bitki.Core.Entities.Ucuyag>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Bitki.Core.Entities.Ucuyag>
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
