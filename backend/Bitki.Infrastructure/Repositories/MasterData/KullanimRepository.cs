using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KullanimRepository : IKullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public KullanimRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "kullanim", "tip", "seviye" };
            var searchableColumns = new[] { "kullanim", "tip" };

            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "UsageName", "kullanim" },
                { "Type", "tip" },
                { "Level", "seviye" }
            };

            _queryBuilder = new QueryBuilder("kullanim", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Kullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Kullanim>("SELECT id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level FROM dbo.kullanim ORDER BY kullanim");
        }

        public async Task<FilterResponse<Kullanim>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.kullanim";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Kullanim>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Kullanim>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Kullanim?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Kullanim>("SELECT id AS Id, kullanim AS UsageName, tip AS Type, seviye AS Level FROM dbo.kullanim WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Kullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.kullanim (kullanim, tip, seviye) VALUES (@UsageName, @Type, @Level) RETURNING id", entity);
        }

        public async Task UpdateAsync(Kullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.kullanim SET kullanim = @UsageName, tip = @Type, seviye = @Level WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.kullanim WHERE id = @Id", new { Id = id });
        }
    }
}
