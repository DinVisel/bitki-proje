using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Cleanup
{
    public class BitkiResimleriRepository : IBitkiResimleriRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public BitkiResimleriRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // Include lowercase UI names, aliased versions, and raw column names
            var allowedColumns = new[] {
                "id", "plantid", "imagelocation", "description", "plantname",
                "br.bitkiresimid", "br.bitkino", "br.resimyeri", "br.aciklama", "b.turkce"
            };
            var searchableColumns = new[] { "br.resimyeri", "br.aciklama", "b.turkce" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "br.bitkiresimid" },
                { "PlantId", "br.bitkino" },
                { "ImageLocation", "br.resimyeri" },
                { "Description", "br.aciklama" },
                { "PlantName", "b.turkce" }
            };
            _queryBuilder = new QueryBuilder("bitkiresimleri", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<BitkiResimleri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiResimleri>(@"
                SELECT br.bitkiresimid AS Id, br.bitkino AS PlantId, br.resimyeri AS ImageLocation, br.aciklama AS Description,
                       b.turkce AS PlantName
                FROM dbo.bitkiresimleri br
                LEFT JOIN dbo.bitki b ON br.bitkino = b.bitkiid
                ORDER BY br.bitkiresimid");
        }

        public async Task<FilterResponse<BitkiResimleri>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = @"
                br.bitkiresimid AS Id, 
                br.bitkino AS PlantId, 
                br.resimyeri AS ImageLocation, 
                br.aciklama AS Description,
                b.turkce AS PlantName";

            var fromClause = "dbo.bitkiresimleri br LEFT JOIN dbo.bitki b ON br.bitkino = b.bitkiid";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, fromClause);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.bitkiresimleri";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, fromClause);

            var data = await connection.QueryAsync<BitkiResimleri>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<BitkiResimleri>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<IEnumerable<BitkiResimleri>> GetByPlantIdAsync(int plantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri WHERE bitkino = @PlantId ORDER BY bitkiresimid", new { PlantId = plantId });
        }

        public async Task<BitkiResimleri?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri WHERE bitkiresimid = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(BitkiResimleri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("INSERT INTO dbo.bitkiresimleri (bitkino, resimyeri, aciklama) VALUES (@PlantId, @ImageLocation, @Description) RETURNING bitkiresimid", entity);
        }

        public async Task UpdateAsync(BitkiResimleri entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.bitkiresimleri SET bitkino = @PlantId, resimyeri = @ImageLocation, aciklama = @Description WHERE bitkiresimid = @Id", entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.bitkiresimleri WHERE bitkiresimid = @Id", new { Id = id });
        }
    }
}
