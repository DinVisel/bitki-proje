using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Taxonomy;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Taxonomy
{
    public class FamilyaRepository : IFamilyaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public FamilyaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "familyaid", "familya", "turkce" };
            var searchableColumns = new[] { "familya", "turkce" };
            _queryBuilder = new QueryBuilder("familya", allowedColumns, searchableColumns);
        }

        public async Task<IEnumerable<Familya>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT familyaid AS Id, familya AS Name, turkce AS TurkishName FROM dbo.familya ORDER BY familya LIMIT 1000";
            return await connection.QueryAsync<Familya>(sql);
        }

        public async Task<FilterResponse<Familya>> QueryAsync(FilterRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "familyaid AS Id, familya AS Name, turkce AS TurkishName";
            var selectSql = _queryBuilder.BuildSelectQuery(
                selectColumns, request.SearchText, request.Filters,
                request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.familya";
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Familya>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Familya>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Familya?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT familyaid AS Id, familya AS Name, turkce AS TurkishName FROM dbo.familya WHERE familyaid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Familya>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Familya entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO dbo.familya (familya, turkce) VALUES (@Name, @TurkishName) RETURNING familyaid";
            return await connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(Familya entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "UPDATE dbo.familya SET familya = @Name, turkce = @TurkishName WHERE familyaid = @Id";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.familya WHERE familyaid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

