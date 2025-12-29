using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteLokaliteRepository : IAktiviteLokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public AktiviteLokaliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "yereladi", "mevki", "sehirno", "ilceno" };
            var searchableColumns = new[] { "yereladi", "mevki" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "LocalName", "yereladi" },
                { "Location", "mevki" },
                { "CityId", "sehirno" },
                { "DistrictId", "ilceno" }
            };
            _queryBuilder = new QueryBuilder("aktivitelokalite", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<AktiviteLokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteLokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId FROM dbo.aktivitelokalite ORDER BY yereladi");
        }

        public async Task<FilterResponse<AktiviteLokalite>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktivitelokalite";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<AktiviteLokalite>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<AktiviteLokalite>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<AktiviteLokalite?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteLokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId FROM dbo.aktivitelokalite WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteLokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitelokalite (yereladi, mevki, sehirno, ilceno) VALUES (@LocalName, @Location, @CityId, @DistrictId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteLokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitelokalite SET yereladi = @LocalName, mevki = @Location, sehirno = @CityId, ilceno = @DistrictId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitelokalite WHERE id = @Id", new { Id = id });
        }
    }
}
