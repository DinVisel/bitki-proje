using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class EtkilerRepository : IEtkilerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public EtkilerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "etkiid", "adi", "latince", "ingilizce", "aciklama" };
            var searchableColumns = new[] { "adi", "latince", "ingilizce", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "etkiid" },
                { "Name", "adi" },
                { "LatinName", "latince" },
                { "EnglishName", "ingilizce" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("etkiler", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Etkiler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etkiler>("SELECT etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description FROM dbo.etkiler ORDER BY adi");
        }

        public async Task<FilterResponse<Etkiler>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.etkiler";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Etkiler>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Etkiler>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Etkiler?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etkiler>("SELECT etkiid AS Id, adi AS Name, latince AS LatinName, ingilizce AS EnglishName, aciklama AS Description FROM dbo.etkiler WHERE etkiid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etkiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etkiler (adi, latince, ingilizce, aciklama) VALUES (@Name, @LatinName, @EnglishName, @Description) RETURNING etkiid", entity);
        }

        public async Task UpdateAsync(Etkiler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etkiler SET adi = @Name, latince = @LatinName, ingilizce = @EnglishName, aciklama = @Description WHERE etkiid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etkiler WHERE etkiid = @Id", new { Id = id });
        }
    }
}
