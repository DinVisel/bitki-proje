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


            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "al.id" },
                { "LocalName", "al.yereladi" },
                { "Location", "al.mevki" },
                { "CityId", "al.sehirno" },
                { "DistrictId", "al.ilceno" },
                { "CityName", "s.sehir" },
                { "DistrictName", "i.ilce" }
            };
            var allowedColumns = new[] { "id", "yereladi", "mevki", "sehirno", "ilceno", "sehir", "ilce" };
            var searchableColumns = new[] { "yereladi", "mevki", "sehir", "ilce" };
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

            var customJoin = "dbo.aktivitelokalite al LEFT JOIN dbo.sehir s ON al.sehirno = s.sehirid LEFT JOIN dbo.ilce i ON al.ilceno = i.ilceid";

            var selectColumns = @"
                al.id AS Id, 
                al.yereladi AS LocalName, 
                al.mevki AS Location, 
                al.sehirno AS CityId, 
                al.ilceno AS DistrictId,
                s.sehir AS CityName,
                i.ilce AS DistrictName";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, customJoin);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktivitelokalite";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, customJoin);

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
