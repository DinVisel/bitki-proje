using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories
{
    public class BitkiRepository : IBitkiRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public BitkiRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // Define allowed and searchable columns
            var allowedColumns = new[] { "bitkiid", "turkce", "bitki", "aciklama" };
            var searchableColumns = new[] { "turkce", "bitki", "aciklama" };
            _queryBuilder = new QueryBuilder("bitki", allowedColumns, searchableColumns);
        }

        public async Task<IEnumerable<Plant>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT 
                    bitkiid AS Id, 
                    turkce AS Name, 
                    bitki AS LatinName, 
                    aciklama AS Description
                FROM dbo.bitki";
            return await connection.QueryAsync<Plant>(sql);
        }

        public async Task<Plant?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT 
                    bitkiid AS Id, 
                    turkce AS Name, 
                    bitki AS LatinName, 
                    aciklama AS Description
                FROM dbo.bitki 
                WHERE bitkiid = @Id";
            return await connection.QueryFirstOrDefaultAsync<Plant>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Plant plant)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                INSERT INTO dbo.bitki (turkce, bitki, aciklama, tibbi, gida, kultur, zehir, tf, adalar, varliksupheli, revizyon, ex, eksikteshis, kontrolok, yayinok) 
                VALUES (@Name, @LatinName, @Description, false, false, false, false, false, false, false, false, false, false, false, false)
                RETURNING bitkiid";
            return await connection.ExecuteScalarAsync<int>(sql, plant);
        }

        public async Task UpdateAsync(Plant plant)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                UPDATE dbo.bitki 
                SET turkce = @Name, 
                    bitki = @LatinName, 
                    aciklama = @Description
                WHERE bitkiid = @Id";
            await connection.ExecuteAsync(sql, plant);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "DELETE FROM dbo.bitki WHERE bitkiid = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<FilterResponse<Plant>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination(); // Validate pagination parameters

            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // Build SELECT query
            var selectColumns = "bitkiid AS Id, turkce AS Name, bitki AS LatinName, aciklama AS Description";
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
            var totalCountSql = "SELECT COUNT(*) FROM dbo.bitki";

            // Build COUNT query for filtered records
            var filteredCountSql = _queryBuilder.BuildCountQuery(
                request.SearchText,
                request.Filters,
                parameters,
                request.IncludeDeleted
            );

            // Execute queries
            var data = await connection.QueryAsync<Plant>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountSql);
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<Plant>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
