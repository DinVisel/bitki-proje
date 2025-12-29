using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class KisilerRepository : IKisilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public KisilerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "kisiid", "adi", "isim", "soyisim" };
            var searchableColumns = new[] { "adi", "isim", "soyisim" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "kisiid" },
                { "FullName", "adi" },
                { "FirstName", "isim" },
                { "LastName", "soyisim" }
            };
            _queryBuilder = new QueryBuilder("kisiler", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Kisiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Kisiler>("SELECT kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName FROM dbo.kisiler ORDER BY isim, soyisim");
        }

        public async Task<FilterResponse<Kisiler>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.kisiler";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Kisiler>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Kisiler>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Kisiler?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Kisiler>("SELECT kisiid AS Id, adi AS FullName, isim AS FirstName, soyisim AS LastName FROM dbo.kisiler WHERE kisiid = @Id", new { Id = id });
        }

        public async Task<long> AddAsync(Kisiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<long>("INSERT INTO dbo.kisiler (adi, isim, soyisim) VALUES (@FullName, @FirstName, @LastName) RETURNING kisiid", entity);
        }

        public async Task UpdateAsync(Kisiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.kisiler SET adi = @FullName, isim = @FirstName, soyisim = @LastName WHERE kisiid = @Id", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.kisiler WHERE kisiid = @Id", new { Id = id });
        }
    }
}
