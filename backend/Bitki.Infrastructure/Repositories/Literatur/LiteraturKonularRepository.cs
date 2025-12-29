using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturKonularRepository : ILiteraturKonularRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public LiteraturKonularRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "konu", "aciklama" };
            var searchableColumns = new[] { "konu", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Topic", "konu" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("literaturkonular", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<LiteraturKonular>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturKonular>("SELECT id AS Id, konu AS Topic, aciklama AS Description FROM dbo.literaturkonular ORDER BY id");
        }

        public async Task<FilterResponse<LiteraturKonular>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, konu AS Topic, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.literaturkonular";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<LiteraturKonular>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<LiteraturKonular>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<LiteraturKonular?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturKonular>("SELECT id AS Id, konu AS Topic, aciklama AS Description FROM dbo.literaturkonular WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturKonular entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturkonular (konu, aciklama) VALUES (@Topic, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturKonular entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturkonular SET konu = @Topic, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturkonular WHERE id = @Id", new { Id = id });
        }
    }
}
