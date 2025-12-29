using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class SehirRepository : ISehirRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public SehirRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "sehirid", "sehir", "trafikkodu" };
            var searchableColumns = new[] { "sehir", "trafikkodu" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "sehirid" },
                { "Name", "sehir" },
                { "TrafficCode", "trafikkodu" }
            };

            _queryBuilder = new QueryBuilder("sehir", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Sehir>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode FROM dbo.sehir ORDER BY sehir";
            return await connection.QueryAsync<Sehir>(sql);
        }

        public async Task<FilterResponse<Sehir>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode";
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

            var totalCountSql = "SELECT COUNT(*) FROM dbo.sehir";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            var data = await connection.QueryAsync<Sehir>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Sehir>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Sehir?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Sehir>("SELECT sehirid AS Id, sehir AS Name, trafikkodu AS TrafficCode FROM dbo.sehir WHERE sehirid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Sehir entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.sehir (sehir, trafikkodu) VALUES (@Name, @TrafficCode) RETURNING sehirid", entity);
        }

        public async Task UpdateAsync(Sehir entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.sehir SET sehir = @Name, trafikkodu = @TrafficCode WHERE sehirid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.sehir WHERE sehirid = @Id", new { Id = id });
        }
    }
}
