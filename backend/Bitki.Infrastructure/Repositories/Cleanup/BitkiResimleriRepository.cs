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

            var allowedColumns = new[] { "bitkiresimid", "bitkino", "resimyeri", "aciklama" };
            var searchableColumns = new[] { "resimyeri", "aciklama" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "bitkiresimid" },
                { "PlantId", "bitkino" },
                { "ImageLocation", "resimyeri" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("bitkiresimleri", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<BitkiResimleri>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<BitkiResimleri>("SELECT bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description FROM dbo.bitkiresimleri ORDER BY bitkiresimid");
        }

        public async Task<FilterResponse<BitkiResimleri>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            var selectColumns = "bitkiresimid AS Id, bitkino AS PlantId, resimyeri AS ImageLocation, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize);

            var totalCountSql = "SELECT COUNT(*) FROM dbo.bitkiresimleri";
            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted);

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
