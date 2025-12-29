using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class OtorRepository : IOtorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public OtorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "otorid", "otor", "aciklama" };
            var searchableColumns = new[] { "otor", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "otorid" },
                { "Name", "otor" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("otor", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Otor>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Otor>("SELECT otorid AS Id, otor AS Name, aciklama AS Description FROM dbo.otor ORDER BY otor");
        }

        public async Task<FilterResponse<Otor>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "otorid AS Id, otor AS Name, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.otor";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Otor>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Otor>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Otor?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Otor>("SELECT otorid AS Id, otor AS Name, aciklama AS Description FROM dbo.otor WHERE otorid = @Id", new { Id = id });
        }

        public async Task<long> AddAsync(Otor entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<long>("INSERT INTO dbo.otor (otor, aciklama) VALUES (@Name, @Description) RETURNING otorid", entity);
        }

        public async Task UpdateAsync(Otor entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.otor SET otor = @Name, aciklama = @Description WHERE otorid = @Id", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.otor WHERE otorid = @Id", new { Id = id });
        }
    }
}
