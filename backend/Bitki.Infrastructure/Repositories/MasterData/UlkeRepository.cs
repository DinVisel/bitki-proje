using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class UlkeRepository : IUlkeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public UlkeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "ulke" };
            var searchableColumns = new[] { "ulke" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Name", "ulke" }
            };

            _queryBuilder = new QueryBuilder("ulke", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Ulke>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, ulke AS Name FROM dbo.ulke ORDER BY ulke";
            return await connection.QueryAsync<Ulke>(sql);
        }

        public async Task<FilterResponse<Ulke>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, ulke AS Name";
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

            var totalCountSql = "SELECT COUNT(*) FROM dbo.ulke";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            var data = await connection.QueryAsync<Ulke>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Ulke>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Ulke?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Ulke>("SELECT id AS Id, ulke AS Name FROM dbo.ulke WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Ulke entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ulke (ulke) VALUES (@Name) RETURNING id", entity);
        }

        public async Task UpdateAsync(Ulke entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ulke SET ulke = @Name WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ulke WHERE id = @Id", new { Id = id });
        }
    }
}
