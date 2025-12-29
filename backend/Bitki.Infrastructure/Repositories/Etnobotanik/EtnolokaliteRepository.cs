using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnolokaliteRepository : IEtnolokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public EtnolokaliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "yereladi", "mevki", "kullanim", "icerik" };
            var searchableColumns = new[] { "yereladi", "mevki", "kullanim", "icerik" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "LocalName", "yereladi" },
                { "Location", "mevki" },
                { "Usage", "kullanim" },
                { "Content", "icerik" }
            };
            _queryBuilder = new QueryBuilder("etnolokalite", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Etnolokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnolokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content FROM dbo.etnolokalite ORDER BY yereladi");
        }

        public async Task<FilterResponse<Etnolokalite>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.etnolokalite";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Etnolokalite>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Etnolokalite>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Etnolokalite?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnolokalite>("SELECT id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content FROM dbo.etnolokalite WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnolokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnolokalite (yereladi, mevki, kullanim, icerik) VALUES (@LocalName, @Location, @Usage, @Content) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnolokalite entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnolokalite SET yereladi = @LocalName, mevki = @Location, kullanim = @Usage, icerik = @Content WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnolokalite WHERE id = @Id", new { Id = id });
        }
    }
}
