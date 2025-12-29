using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Taxonomy;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Taxonomy
{
    public class GenusRepository : IGenusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public GenusRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "genusid", "genus", "familyano", "aciklama" };
            var searchableColumns = new[] { "genus", "aciklama" };
            _queryBuilder = new QueryBuilder("genus", allowedColumns, searchableColumns);
        }

        public async Task<IEnumerable<Genus>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT genusid AS Id, genus AS Name, familyano AS FamilyId, aciklama AS Description FROM dbo.genus ORDER BY genus LIMIT 1000";
            return await connection.QueryAsync<Genus>(sql);
        }

        public async Task<FilterResponse<Genus>> QueryAsync(FilterRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "genusid AS Id, genus AS Name, familyano AS FamilyId, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters,
                request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.genus";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<Genus>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Genus>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Genus?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Genus>(
                "SELECT genusid AS Id, genus AS Name, familyano AS FamilyId, aciklama AS Description FROM dbo.genus WHERE genusid = @Id",
                new { Id = id });
        }

        public async Task<int> AddAsync(Genus entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "INSERT INTO dbo.genus (genus, familyano, aciklama) VALUES (@Name, @FamilyId, @Description) RETURNING genusid", entity);
        }

        public async Task UpdateAsync(Genus entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "UPDATE dbo.genus SET genus = @Name, familyano = @FamilyId, aciklama = @Description WHERE genusid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.genus WHERE genusid = @Id", new { Id = id });
        }
    }
}

