using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class IlceRepository : IIlceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public IlceRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "ilceid", "ilce", "sehirno", "sehir" };
            var searchableColumns = new[] { "ilce", "sehir" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "i.ilceid" },
                { "Name", "i.ilce" },
                { "CityId", "i.sehirno" },
                { "CityName", "s.sehir" }
            };

            _queryBuilder = new QueryBuilder("ilce", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Ilce>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT ilceid AS Id, ilce AS Name, sehirno AS CityId FROM dbo.ilce ORDER BY ilce LIMIT 1000";
            return await connection.QueryAsync<Ilce>(sql);
        }

        public async Task<FilterResponse<Ilce>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "i.ilceid AS Id, i.ilce AS Name, i.sehirno AS CityId, s.sehir AS CityName";
            var customJoin = "dbo.ilce i LEFT JOIN dbo.sehir s ON i.sehirno = s.sehirid";

            var selectSql = _queryBuilder.BuildSelectQuery(
                selectColumns,
                request.SearchText,
                request.Filters,
                request.SortColumn,
                request.SortDirection,
                parameters,
                request.IncludeDeleted,
                request.PageNumber,
                request.PageSize,
                customJoin
            );

            var totalCountSql = "SELECT COUNT(*) FROM dbo.ilce";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted,
                customJoin
            );

            var data = await connection.QueryAsync<Ilce>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Ilce>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Ilce?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Ilce>("SELECT ilceid AS Id, ilce AS Name, sehirno AS CityId FROM dbo.ilce WHERE ilceid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Ilce entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ilce (ilce, sehirno) VALUES (@Name, @CityId) RETURNING ilceid", entity);
        }

        public async Task UpdateAsync(Ilce entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ilce SET ilce = @Name, sehirno = @CityId WHERE ilceid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ilce WHERE ilceid = @Id", new { Id = id });
        }
    }
}
