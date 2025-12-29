using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteBitkilitRepository : IAktiviteBitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public AktiviteBitkilitRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "turkcead", "durum", "litno", "familyano", "genusno" };
            var searchableColumns = new[] { "turkcead", "durum" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "id" },
                { "TurkishName", "turkcead" },
                { "Status", "durum" },
                { "LiteratureId", "litno" },
                { "FamilyId", "familyano" },
                { "GenusId", "genusno" }
            };
            _queryBuilder = new QueryBuilder("aktivitebitkilit", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<AktiviteBitkilit>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AktiviteBitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId FROM dbo.aktivitebitkilit ORDER BY turkcead");
        }

        public async Task<FilterResponse<AktiviteBitkilit>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.aktivitebitkilit";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

            var data = await connection.QueryAsync<AktiviteBitkilit>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<AktiviteBitkilit>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<AktiviteBitkilit?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AktiviteBitkilit>("SELECT id AS Id, turkcead AS TurkishName, durum AS Status, litno AS LiteratureId, familyano AS FamilyId, genusno AS GenusId FROM dbo.aktivitebitkilit WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(AktiviteBitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.aktivitebitkilit (turkcead, durum, litno, familyano, genusno) VALUES (@TurkishName, @Status, @LiteratureId, @FamilyId, @GenusId) RETURNING id", entity);
        }

        public async Task UpdateAsync(AktiviteBitkilit entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.aktivitebitkilit SET turkcead = @TurkishName, durum = @Status, litno = @LiteratureId, familyano = @FamilyId, genusno = @GenusId WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.aktivitebitkilit WHERE id = @Id", new { Id = id });
        }
    }
}
