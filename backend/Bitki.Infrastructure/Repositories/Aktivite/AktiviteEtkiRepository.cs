using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteEtkiRepository : IAktiviteEtkiRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public AktiviteEtkiRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "adi", "ingilizce", "aciklama" };
            var searchableColumns = new[] { "adi", "ingilizce", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Name", "adi" },
                { "EnglishName", "ingilizce" },
                { "Description", "aciklama" }
            };

            _queryBuilder = new QueryBuilder("aktiviteetki", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<AktiviteEtki>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteEtki>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktiviteetki ORDER BY adi");
        }

        public async Task<FilterResponse<AktiviteEtki>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description";
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

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktiviteetki";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            var data = await connection.QueryAsync<AktiviteEtki>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<AktiviteEtki>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<AktiviteEtki?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteEtki>("SELECT id AS Id, adi AS Name, ingilizce AS EnglishName, aciklama AS Description FROM dbo.aktiviteetki WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteEtki entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktiviteetki (adi, ingilizce, aciklama) VALUES (@Name, @EnglishName, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteEtki entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktiviteetki SET adi = @Name, ingilizce = @EnglishName, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktiviteetki WHERE id = @Id", new { Id = id });
        }
    }
}
