using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturHatalariRepository : ILiteraturHatalariRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public LiteraturHatalariRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "hataadi", "aciklama" };
            var searchableColumns = new[] { "hataadi", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "ErrorName", "hataadi" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("literaturhatalari", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<LiteraturHatalari>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturHatalari>("SELECT id AS Id, hataadi AS ErrorName, aciklama AS Description FROM dbo.literaturhatalari ORDER BY id");
        }

        public async Task<FilterResponse<LiteraturHatalari>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, hataadi AS ErrorName, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.literaturhatalari";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<LiteraturHatalari>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<LiteraturHatalari>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<LiteraturHatalari?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturHatalari>("SELECT id AS Id, hataadi AS ErrorName, aciklama AS Description FROM dbo.literaturhatalari WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturHatalari entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturhatalari (hataadi, aciklama) VALUES (@ErrorName, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturHatalari entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturhatalari SET hataadi = @ErrorName, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturhatalari WHERE id = @Id", new { Id = id });
        }
    }
}
