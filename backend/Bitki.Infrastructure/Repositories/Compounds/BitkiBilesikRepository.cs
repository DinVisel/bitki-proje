using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BitkiBilesikRepository : IBitkiBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public BitkiBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "amount", "description", "plantname", "compoundname", "b.turkce", "bl.adi", "bb.miktar", "bb.aciklama" };
            var searchableColumns = new[] { "b.turkce", "bl.adi", "bb.aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Amount", "bb.miktar" },
                { "Description", "bb.aciklama" },
                { "PlantName", "b.turkce" },
                { "CompoundName", "bl.adi" }
            };
            _queryBuilder = new QueryBuilder("bitkibilesik", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<BitkiBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiBilesik>(@"
                SELECT bb.id AS Id, bb.bitkino AS PlantId, bb.bilesikno AS CompoundId, bb.miktar AS Amount, bb.aciklama AS Description,
                       b.turkce AS PlantName, bl.adi AS CompoundName
                FROM dbo.bitkibilesik bb
                LEFT JOIN dbo.bitki b ON bb.bitkino = b.bitkiid
                LEFT JOIN dbo.bilesikler bl ON bb.bilesikno = bl.bilesikid
                ORDER BY bb.id DESC");
        }

        public async Task<FilterResponse<BitkiBilesik>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // FORCE DEFAULT SORT if not provided or invalid
            if (string.IsNullOrWhiteSpace(request.SortColumn) || request.SortColumn.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                request.SortColumn = "PlantName";
            }

            var selectColumns = @"
                bb.id AS Id, 
                bb.bitkino AS PlantId, 
                bb.bilesikno AS CompoundId, 
                bb.miktar AS Amount, 
                bb.aciklama AS Description,
                b.turkce AS PlantName,
                bl.adi AS CompoundName";

            var fromClause = @"
                dbo.bitkibilesik bb
                LEFT JOIN dbo.bitki b ON bb.bitkino = b.bitkiid
                LEFT JOIN dbo.bilesikler bl ON bb.bilesikno = bl.bilesikid";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, fromClause);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.bitkibilesik";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, fromClause);

            var data = await connection.QueryAsync<BitkiBilesik>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<BitkiBilesik>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<BitkiBilesik?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BitkiBilesik>("SELECT id AS Id, bitkino AS PlantId, bilesikno AS CompoundId, miktar AS Amount, aciklama AS Description FROM dbo.bitkibilesik WHERE id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(BitkiBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.bitkibilesik (bitkino, bilesikno, miktar, aciklama) VALUES (@PlantId, @CompoundId, @Amount, @Description) RETURNING id", entity);
        }

        public async Task UpdateAsync(BitkiBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.bitkibilesik SET bitkino = @PlantId, bilesikno = @CompoundId, miktar = @Amount, aciklama = @Description WHERE id = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.bitkibilesik WHERE id = @Id", new { Id = id });
        }
    }
}
