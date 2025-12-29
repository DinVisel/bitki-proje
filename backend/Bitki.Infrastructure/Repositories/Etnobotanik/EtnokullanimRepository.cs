using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnokullanimRepository : IEtnokullanimRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public EtnokullanimRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "aciklama", "lokaliteno", "tariholusturma" };
            var searchableColumns = new[] { "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "Description", "aciklama" },
                { "LocalityId", "lokaliteno" },
                { "CreatedDate", "tariholusturma" }
            };
            _queryBuilder = new QueryBuilder("etnokullanim", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Etnokullanim>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Etnokullanim>("SELECT id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate FROM dbo.etnokullanim ORDER BY id");
        }

        public async Task<FilterResponse<Etnokullanim>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.etnokullanim";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Etnokullanim>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Etnokullanim>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Etnokullanim?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Etnokullanim>("SELECT id AS Id, aciklama AS Description, lokaliteno AS LocalityId, tariholusturma AS CreatedDate FROM dbo.etnokullanim WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Etnokullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.etnokullanim (aciklama, lokaliteno, tariholusturma) VALUES (@Description, @LocalityId, @CreatedDate) RETURNING id", entity);
        }

        public async Task UpdateAsync(Etnokullanim entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.etnokullanim SET aciklama = @Description, lokaliteno = @LocalityId, tariholusturma = @CreatedDate WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.etnokullanim WHERE id = @Id", new { Id = id });
        }
    }
}
