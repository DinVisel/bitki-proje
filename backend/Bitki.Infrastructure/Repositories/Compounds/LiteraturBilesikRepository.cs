using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class LiteraturBilesikRepository : ILiteraturBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public LiteraturBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "id", "literaturno", "bilesikno", "aciklama", "baslik", "adi" };
            var searchableColumns = new[] { "aciklama", "l.baslik", "b.adi" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "lb.id" },
                { "LiteratureId", "lb.literaturno" },
                { "CompoundId", "lb.bilesikno" },
                { "Description", "lb.aciklama" },
                { "LiteratureName", "l.baslik" },
                { "CompoundName", "b.adi" }
            };
            _queryBuilder = new QueryBuilder("literaturbilesik", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<LiteraturBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<LiteraturBilesik>(@"
                SELECT lb.id AS Id, lb.literaturno AS LiteratureId, lb.bilesikno AS CompoundId, lb.aciklama AS Description,
                       l.baslik AS LiteratureName, b.adi AS CompoundName
                FROM dbo.literaturbilesik lb
                LEFT JOIN dbo.literatur l ON lb.literaturno = l.id
                LEFT JOIN dbo.bilesikler b ON lb.bilesikno = b.id
                ORDER BY lb.id DESC");
        }

        public async Task<FilterResponse<LiteraturBilesik>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = @"
                lb.id AS Id, 
                lb.literaturno AS LiteratureId, 
                lb.bilesikno AS CompoundId, 
                lb.aciklama AS Description,
                l.baslik AS LiteratureName,
                b.adi AS CompoundName";

            var fromClause = @"
                dbo.literaturbilesik lb
                LEFT JOIN dbo.literatur l ON lb.literaturno = l.id
                LEFT JOIN dbo.bilesikler b ON lb.bilesikno = b.id";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, fromClause);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.literaturbilesik";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, fromClause);

            var data = await connection.QueryAsync<LiteraturBilesik>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<LiteraturBilesik>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<LiteraturBilesik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LiteraturBilesik>("SELECT id AS Id, literaturno AS LiteratureId, bilesikno AS CompoundId, aciklama AS Description FROM dbo.literaturbilesik WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(LiteraturBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.literaturbilesik (literaturno, bilesikno, aciklama) VALUES (@LiteratureId, @CompoundId, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(LiteraturBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.literaturbilesik SET literaturno = @LiteratureId, bilesikno = @CompoundId, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.literaturbilesik WHERE id = @Id", new { Id = id });
        }
    }
}
