using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class OzellikRepository : IOzellikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public OzellikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "ozellikid", "adi", "tipno", "aciklama" };
            var searchableColumns = new[] { "adi", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "ozellikid" },
                { "Name", "adi" },
                { "TypeId", "tipno" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("ozellik", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Bitki.Core.Entities.Ozellik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Bitki.Core.Entities.Ozellik>("SELECT ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description FROM dbo.ozellik ORDER BY adi");
        }

        public async Task<FilterResponse<Bitki.Core.Entities.Ozellik>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.ozellik";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Bitki.Core.Entities.Ozellik>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Bitki.Core.Entities.Ozellik>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Bitki.Core.Entities.Ozellik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Bitki.Core.Entities.Ozellik>("SELECT ozellikid AS Id, adi AS Name, tipno AS TypeId, aciklama AS Description FROM dbo.ozellik WHERE ozellikid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Bitki.Core.Entities.Ozellik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.ozellik (adi, tipno, aciklama) VALUES (@Name, @TypeId, @Description) RETURNING ozellikid", entity);
        }

        public async Task UpdateAsync(Bitki.Core.Entities.Ozellik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ozellik SET adi = @Name, tipno = @TypeId, aciklama = @Description WHERE ozellikid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ozellik WHERE ozellikid = @Id", new { Id = id });
        }
    }
}
