using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteYontemRepository : IAktiviteYontemRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public AktiviteYontemRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "adi", "aciklama" };
            var searchableColumns = new[] { "adi", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Name", "adi" },
                { "Description", "aciklama" }
            };

            _queryBuilder = new QueryBuilder("aktiviteyontem", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<AktiviteYontem>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteYontem>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktiviteyontem ORDER BY adi");
        }

        public async Task<FilterResponse<AktiviteYontem>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, adi AS Name, aciklama AS Description";
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

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktiviteyontem";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            var data = await connection.QueryAsync<AktiviteYontem>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<AktiviteYontem>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<AktiviteYontem?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteYontem>("SELECT id AS Id, adi AS Name, aciklama AS Description FROM dbo.aktiviteyontem WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteYontem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktiviteyontem (adi, aciklama) VALUES (@Name, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteYontem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktiviteyontem SET adi = @Name, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktiviteyontem WHERE id = @Id", new { Id = id });
        }
    }
}
