using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Ucuyag;
using Bitki.Core.Entities; // Assuming Ucuyag entity is here
using Bitki.Core.Models;
using Bitki.Core.Utilities; // For QueryBuilder
using Dapper;

namespace Bitki.Infrastructure.Repositories.Ucuyag
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

            // Map UI property names → DB column names
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Name", "ucucuyagadi" },
                { "LocalName", "yereladi" },
                { "Usage", "kullanim" }
            };
            _queryBuilder = new QueryBuilder("ucuyag", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ucucuyagadi AS Name, yereladi AS LocalName, kullanim AS Usage FROM dbo.ucuyag ORDER BY ucucuyagadi LIMIT 1000";
            return await connection.QueryAsync<Bitki.Core.Entities.Ucuyag>(sql);
        }

        public async Task<FilterResponse<Bitki.Core.Entities.Ucuyag>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

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

            var totalCountSql = "SELECT COUNT(*) FROM dbo.ucuyag";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

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

        public async Task<Bitki.Core.Entities.Ucuyag?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ucucuyagadi AS Name, yereladi AS LocalName, kullanim AS Usage FROM dbo.ucuyag WHERE id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Bitki.Core.Entities.Ucuyag>(sql, new { Id = id });
        }

        public async Task<long> AddAsync(Bitki.Core.Entities.Ucuyag entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO dbo.ucuyag (ucucuyagadi, yereladi, kullanim) 
                        VALUES (@Name, @LocalName, @Usage) 
                        RETURNING id";
            return await connection.ExecuteScalarAsync<long>(sql, entity);
        }

        public async Task UpdateAsync(Bitki.Core.Entities.Ucuyag entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"UPDATE dbo.ucuyag 
                        SET ucucuyagadi = @Name, yereladi = @LocalName, kullanim = @Usage 
                        WHERE id = @Id";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.ucuyag WHERE id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
