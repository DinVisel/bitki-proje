using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class UcucuYagBilesikRepository : IUcucuYagBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public UcucuYagBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            var allowedColumns = new[] { "amount", "unit", "essentialoilname", "compoundname", "uy.ucucuyagadi", "bl.adi", "uyb.miktar", "uyb.birim" };
            var searchableColumns = new[] { "uy.ucucuyagadi", "bl.adi", "uyb.birim" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Amount", "uyb.miktar" },
                { "Unit", "uyb.birim" },
                { "EssentialOilName", "uy.ucucuyagadi" },
                { "CompoundName", "bl.adi" }
            };
            _queryBuilder = new QueryBuilder("ucucuyagbilesik", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<IEnumerable<UcucuYagBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<UcucuYagBilesik>(@"
                SELECT uyb.ucucuyagno AS EssentialOilId, uyb.bilesikno AS CompoundId, uyb.miktar AS Amount, uyb.birim AS Unit,
                       uy.ucucuyagadi AS EssentialOilName, bl.adi AS CompoundName
                FROM dbo.ucucuyagbilesik uyb
                LEFT JOIN dbo.ucuyag uy ON uyb.ucucuyagno = uy.id
                LEFT JOIN dbo.bilesikler bl ON uyb.bilesikno = bl.bilesikid");
        }

        public async Task<FilterResponse<UcucuYagBilesik>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // FORCE DEFAULT SORT if not provided or invalid
            if (string.IsNullOrWhiteSpace(request.SortColumn) || request.SortColumn.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                request.SortColumn = "EssentialOilName";
            }

            var selectColumns = @"
                uyb.ucucuyagno AS EssentialOilId, 
                uyb.bilesikno AS CompoundId, 
                uyb.miktar AS Amount, 
                uyb.birim AS Unit,
                uy.ucucuyagadi AS EssentialOilName, 
                bl.adi AS CompoundName";

            var fromClause = @"
                dbo.ucucuyagbilesik uyb
                LEFT JOIN dbo.ucuyag uy ON uyb.ucucuyagno = uy.id
                LEFT JOIN dbo.bilesikler bl ON uyb.bilesikno = bl.bilesikid";

            var selectSql = _queryBuilder.BuildSelectQuery(selectColumns, request.SearchText, request.Filters, request.SortColumn, request.SortDirection, parameters, request.IncludeDeleted, request.PageNumber, request.PageSize, fromClause);

            var filteredCountSql = _queryBuilder.BuildCountQuery(request.SearchText, request.Filters, parameters, request.IncludeDeleted, fromClause);

            var data = await connection.QueryAsync<UcucuYagBilesik>(selectSql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.ucucuyagbilesik");
            var filteredCount = await connection.ExecuteScalarAsync<int>(filteredCountSql, parameters);

            return new FilterResponse<UcucuYagBilesik>
            {
                Data = data,
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<UcucuYagBilesik?> GetByIdAsync(int essentialOilId, int compoundId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UcucuYagBilesik>("SELECT ucucuyagno AS EssentialOilId, bilesikno AS CompoundId, miktar AS Amount, birim AS Unit FROM dbo.ucucuyagbilesik WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", new { EssentialOilId = essentialOilId, CompoundId = compoundId });
        }

        public async Task AddAsync(UcucuYagBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("INSERT INTO dbo.ucucuyagbilesik (ucucuyagno, bilesikno, miktar, birim) VALUES (@EssentialOilId, @CompoundId, @Amount, @Unit)", entity);
        }

        public async Task UpdateAsync(UcucuYagBilesik entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE dbo.ucucuyagbilesik SET miktar = @Amount, birim = @Unit WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", entity);
        }

        public async Task DeleteAsync(int essentialOilId, int compoundId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM dbo.ucucuyagbilesik WHERE ucucuyagno = @EssentialOilId AND bilesikno = @CompoundId", new { EssentialOilId = essentialOilId, CompoundId = compoundId });
        }
    }
}
