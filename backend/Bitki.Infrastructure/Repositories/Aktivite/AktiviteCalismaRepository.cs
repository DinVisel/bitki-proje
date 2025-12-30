using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteCalismaRepository : IAktiviteCalismaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public AktiviteCalismaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;


            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "ac.id" },
                { "Description", "ac.aciklama" },
                { "CreatedDate", "ac.tariholusturma" },
                { "LocalityId", "ac.lokaliteno" },
                { "EffectId", "ac.etkino" },
                { "LocalityName", "al.yereladi" },
                { "EffectName", "ae.adi" }
            };
            var allowedColumns = new[] { "id", "aciklama", "tariholusturma", "lokaliteno", "etkino", "yereladi", "adi", "ac.id", "ac.aciklama", "ac.tariholusturma", "ac.lokaliteno", "ac.etkino", "al.yereladi", "ae.adi" };
            var searchableColumns = new[] { "aciklama", "yereladi", "adi" };
            _queryBuilder = new QueryBuilder("aktivitecalisma", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<AktiviteCalisma>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteCalisma>("SELECT id AS Id, aciklama AS Description, tariholusturma AS CreatedDate, lokaliteno AS LocalityId, etkino AS EffectId FROM dbo.aktivitecalisma ORDER BY id DESC");
        }

        public async Task<FilterResponse<AktiviteCalisma>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var customJoin = "dbo.aktivitecalisma ac LEFT JOIN dbo.aktivitelokalite al ON ac.lokaliteno = al.id LEFT JOIN dbo.aktiviteetki ae ON ac.etkino = ae.id";

            var selectColumns = @"
                ac.id AS Id, 
                ac.aciklama AS Description, 
                ac.tariholusturma AS CreatedDate, 
                ac.lokaliteno AS LocalityId, 
                ac.etkino AS EffectId,
                al.yereladi AS LocalityName,
                ae.adi AS EffectName";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, customJoin);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktivitecalisma";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, customJoin);

            var data = await connection.QueryAsync<AktiviteCalisma>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<AktiviteCalisma>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<AktiviteCalisma?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteCalisma>("SELECT id AS Id, aciklama AS Description, tariholusturma AS CreatedDate, lokaliteno AS LocalityId, etkino AS EffectId FROM dbo.aktivitecalisma WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteCalisma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitecalisma (aciklama, tariholusturma, lokaliteno, etkino) VALUES (@Description, @CreatedDate, @LocalityId, @EffectId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteCalisma entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitecalisma SET aciklama = @Description, tariholusturma = @CreatedDate, lokaliteno = @LocalityId, etkino = @EffectId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitecalisma WHERE id = @Id", new { Id = id });
        }
    }
}
