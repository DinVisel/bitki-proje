using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnobitkilitRepository : IEtnobitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public EtnobitkilitRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "turkcead", "durum", "litno" };
            var searchableColumns = new[] { "turkcead", "durum" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "TurkishName", "turkcead" },
                { "Status", "durum" },
                { "LiteratureId", "litno" }
            };
            _queryBuilder = new QueryBuilder("etnobitkilit", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Etnobitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnobitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId FROM dbo.etnobitkilit ORDER BY turkcead");
        }

        public async Task<FilterResponse<Etnobitkilit>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.etnobitkilit";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Etnobitkilit>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Etnobitkilit>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Etnobitkilit?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnobitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId FROM dbo.etnobitkilit WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnobitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnobitkilit (turkcead, durum, litno) VALUES (@TurkishName, @Status, @LiteratureId) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnobitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnobitkilit SET turkcead = @TurkishName, durum = @Status, litno = @LiteratureId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnobitkilit WHERE id = @Id", new { Id = id });
        }
    }
}
