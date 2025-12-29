using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BilesiklerRepository : IBilesiklerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public BilesiklerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // Define allowed and searchable columns
            var allowedColumns = new[] { "bilesikid", "adi", "ingilizce", "latince", "aciklama" };
            var searchableColumns = new[] { "adi", "ingilizce", "latince", "aciklama" };

            // Map UI property names → DB column names
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "bilesikid" },
                { "Name", "adi" },
                { "EnglishName", "ingilizce" },
                { "LatinName", "latince" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("bilesikler", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<Bilesikler>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description FROM dbo.bilesikler ORDER BY adi";
            return await connection.QueryAsync<Bilesikler>(sql);
        }

        public async Task<FilterResponse<Bilesikler>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // Build SELECT query
            var selectColumns = "bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description";
            var selectSql = _queryBuilder.BuildSelectQuery(
                selectColumns,
                request.SearchText,
                request.Filters,
                request.SortColumn,
                request.SortDirection,
                parameters,
                request.IncludeDeleted,
                request.PageNumber,
                request.PageSize
            );

            // Build COUNT query for total records
            var totalCountSql = "SELECT COUNT(*) FROM dbo.bilesikler";

            // Build COUNT query for filtered records
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            // Execute queries
            var data = await connection.QueryAsync<Bilesikler>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Bilesikler>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Bilesikler?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT bilesikid AS Id, adi AS Name, ingilizce AS EnglishName, latince AS LatinName, aciklama AS Description FROM dbo.bilesikler WHERE bilesikid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Bilesikler>(sql, new { Id = id });
        }

        public async Task<long> AddAsync(Bilesikler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO dbo.bilesikler (adi, ingilizce, latince, aciklama) 
                        VALUES (@Name, @EnglishName, @LatinName, @Description) 
                        RETURNING bilesikid";
            return await connection.ExecuteScalarAsync<long>(sql, entity);
        }

        public async Task UpdateAsync(Bilesikler entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"UPDATE dbo.bilesikler 
                        SET adi = @Name, ingilizce = @EnglishName, latince = @LatinName, aciklama = @Description 
                        WHERE bilesikid = @Id";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.bilesikler WHERE bilesikid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

