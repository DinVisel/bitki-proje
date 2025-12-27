using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.DTOs;
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

            // Map UI property names → DB column names
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "bitkiid" },
                { "Name", "turkce" },
                { "LatinName", "bitki" },
                { "Description", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("bitki", allowedColumns, searchableColumns, columnMappings);
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

        public async Task<BitkiDetailDto?> GetDetailByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Main query with available bitki fields and JOINs for taxonomy
            // Note: bitki -> genus -> familya relationship
            var mainSql = @"
                SELECT 
                    b.bitkiid AS Id,
                    b.turkce AS TurkishName,
                    b.bitki AS LatinName,
                    b.aciklama AS Description,
                    g.familyano AS FamilyId,
                    f.familya AS FamilyName,
                    f.turkce AS FamilyTurkishName,
                    b.genusno AS GenusId,
                    g.genus AS GenusName,
                    COALESCE(b.tibbi, false) AS IsMedicinal,
                    COALESCE(b.gida, false) AS IsFood,
                    COALESCE(b.kultur, false) AS IsCultural,
                    COALESCE(b.zehir, false) AS IsPoisonous,
                    COALESCE(b.tf, false) AS IsTurkishFlora,
                    COALESCE(b.adalar, false) AS IsIslandSpecies,
                    COALESCE(b.varliksupheli, false) AS ExistenceDoubtful,
                    COALESCE(b.revizyon, false) AS NeedsRevision,
                    COALESCE(b.ex, false) AS IsExtinct,
                    COALESCE(b.eksikteshis, false) AS IncompleteIdentification,
                    COALESCE(b.kontrolok, false) AS ControlOk,
                    COALESCE(b.yayinok, false) AS PublicationOk,
                    b.endemizm AS Endemism,
                    b.revizyonaciklama AS EndemismDescription,
                    CASE 
                        WHEN b.ilkcicek IS NOT NULL AND b.soncicek IS NOT NULL 
                        THEN CONCAT(b.ilkcicek::text, '-', b.soncicek::text)
                        ELSE NULL 
                    END AS FloweringTime,
                    b.hayatformu AS Habitat,
                    CASE 
                        WHEN b.minyuseklik IS NOT NULL AND b.maxyukseklik IS NOT NULL 
                        THEN CONCAT(b.minyuseklik::text, '-', b.maxyukseklik::text, ' m')
                        ELSE NULL 
                    END AS Altitude,
                    CONCAT(COALESCE(b.tdagilim, ''), ' ', COALESCE(b.ddagilim, '')) AS Distribution,
                    b.davis AS Phytogeography,
                    b.sinonimler AS CommonNames,
                    b.davis AS Notes,
                    b.species AS TaxonName,
                    b.subspecies AS TaxonKind
                FROM dbo.bitki b
                LEFT JOIN dbo.genus g ON b.genusno = g.genusid
                LEFT JOIN dbo.familya f ON g.familyano = f.familyaid
                WHERE b.bitkiid = @Id";

            var detail = await connection.QueryFirstOrDefaultAsync<BitkiDetailDto>(mainSql, new { Id = id });


            if (detail == null)
                return null;

            // Get related compounds with names
            var compoundsSql = @"
                SELECT 
                    bb.id AS Id,
                    bb.bilesikno AS CompoundId,
                    bl.adi AS CompoundName,
                    bl.ingilizce AS CompoundEnglishName,
                    bl.latince AS CompoundLatinName,
                    bb.miktar AS Amount,
                    bb.aciklama AS Description
                FROM dbo.bitkibilesik bb
                LEFT JOIN dbo.bilesikler bl ON bb.bilesikno = bl.bilesikid
                WHERE bb.bitkino = @Id
                ORDER BY bl.adi";


            var compounds = await connection.QueryAsync<PlantCompoundDto>(compoundsSql, new { Id = id });
            detail.Compounds = compounds.ToList();

            // Get related images
            var imagesSql = @"
                SELECT 
                    bitkiresimid AS Id,
                    resimyeri AS ImageLocation,
                    aciklama AS Description
                FROM dbo.bitkiresimleri
                WHERE bitkino = @Id
                ORDER BY bitkiresimid";

            var images = await connection.QueryAsync<PlantImageDto>(imagesSql, new { Id = id });
            detail.Images = images.ToList();

            // Get related literature (if there's a relation table)
            // Note: This may need adjustment based on actual schema
            var literatureSql = @"
                SELECT DISTINCT
                    l.literaturid AS Id,
                    l.yazarad AS AuthorName,
                    l.arastirmaadi AS ResearchName,
                    l.kaynakadi AS SourceName,
                    l.yil AS Year,
                    l.tur AS Type
                FROM dbo.literatur l
                WHERE l.literaturid IN (
                    SELECT DISTINCT litno FROM dbo.etnobitkilit WHERE turkcead ILIKE '%' || @TurkishName || '%'
                    UNION
                    SELECT DISTINCT litno FROM dbo.aktivitebitkilit WHERE turkcead ILIKE '%' || @TurkishName || '%'
                )
                LIMIT 50";

            try
            {
                var literature = await connection.QueryAsync<PlantLiteratureDto>(literatureSql, new { TurkishName = detail.TurkishName ?? "" });
                detail.Literature = literature.ToList();
            }
            catch
            {
                // If literature query fails, just return empty list
                detail.Literature = new List<PlantLiteratureDto>();
            }

            return detail;
        }

        /// <summary>
        /// Get basic detail without related data (for lazy loading)
        /// </summary>
        public async Task<BitkiDetailDto?> GetBasicDetailByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var mainSql = @"
                SELECT 
                    b.bitkiid AS Id,
                    b.turkce AS TurkishName,
                    b.bitki AS LatinName,
                    b.aciklama AS Description,
                    g.familyano AS FamilyId,
                    f.familya AS FamilyName,
                    f.turkce AS FamilyTurkishName,
                    b.genusno AS GenusId,
                    g.genus AS GenusName,
                    COALESCE(b.tibbi, false) AS IsMedicinal,
                    COALESCE(b.gida, false) AS IsFood,
                    COALESCE(b.kultur, false) AS IsCultural,
                    COALESCE(b.zehir, false) AS IsPoisonous,
                    COALESCE(b.tf, false) AS IsTurkishFlora,
                    COALESCE(b.adalar, false) AS IsIslandSpecies,
                    COALESCE(b.varliksupheli, false) AS ExistenceDoubtful,
                    COALESCE(b.revizyon, false) AS NeedsRevision,
                    COALESCE(b.ex, false) AS IsExtinct,
                    COALESCE(b.eksikteshis, false) AS IncompleteIdentification,
                    COALESCE(b.kontrolok, false) AS ControlOk,
                    COALESCE(b.yayinok, false) AS PublicationOk,
                    b.endemizm AS Endemism,
                    b.revizyonaciklama AS EndemismDescription,
                    CASE 
                        WHEN b.ilkcicek IS NOT NULL AND b.soncicek IS NOT NULL 
                        THEN CONCAT(b.ilkcicek::text, '-', b.soncicek::text)
                        ELSE NULL 
                    END AS FloweringTime,
                    b.hayatformu AS Habitat,
                    CASE 
                        WHEN b.minyuseklik IS NOT NULL AND b.maxyukseklik IS NOT NULL 
                        THEN CONCAT(b.minyuseklik::text, '-', b.maxyukseklik::text, ' m')
                        ELSE NULL 
                    END AS Altitude,
                    CONCAT(COALESCE(b.tdagilim, ''), ' ', COALESCE(b.ddagilim, '')) AS Distribution,
                    b.davis AS Phytogeography,
                    b.sinonimler AS CommonNames,
                    b.davis AS Notes,
                    b.species AS TaxonName,
                    b.subspecies AS TaxonKind,
                    (SELECT COUNT(*) FROM dbo.bitkibilesik WHERE bitkino = @Id) AS CompoundsCount,
                    (SELECT COUNT(*) FROM dbo.bitkiresimleri WHERE bitkino = @Id) AS ImagesCount
                FROM dbo.bitki b
                LEFT JOIN dbo.genus g ON b.genusno = g.genusid
                LEFT JOIN dbo.familya f ON g.familyano = f.familyaid
                WHERE b.bitkiid = @Id";

            var detail = await connection.QueryFirstOrDefaultAsync<BitkiDetailDto>(mainSql, new { Id = id });

            if (detail != null)
            {
                // Initialize empty collections for lazy loading
                detail.Compounds = new List<PlantCompoundDto>();
                detail.Images = new List<PlantImageDto>();
                detail.Literature = new List<PlantLiteratureDto>();
            }

            return detail;
        }

        /// <summary>
        /// Get compounds for a plant (lazy loading)
        /// </summary>
        public async Task<IEnumerable<PlantCompoundDto>> GetCompoundsByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    bb.id AS Id,
                    bb.bilesikno AS CompoundId,
                    bl.adi AS CompoundName,
                    bl.ingilizce AS CompoundEnglishName,
                    bl.latince AS CompoundLatinName,
                    bb.miktar AS Amount,
                    bb.aciklama AS Description
                FROM dbo.bitkibilesik bb
                LEFT JOIN dbo.bilesikler bl ON bb.bilesikno = bl.bilesikid
                WHERE bb.bitkino = @Id
                ORDER BY bl.adi";

            return await connection.QueryAsync<PlantCompoundDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Get images for a plant (lazy loading)
        /// </summary>
        public async Task<IEnumerable<PlantImageDto>> GetImagesByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    bitkiresimid AS Id,
                    resimyeri AS ImageLocation,
                    aciklama AS Description
                FROM dbo.bitkiresimleri
                WHERE bitkino = @Id
                ORDER BY bitkiresimid";

            return await connection.QueryAsync<PlantImageDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Get literature for a plant (lazy loading)
        /// </summary>
        public async Task<IEnumerable<PlantLiteratureDto>> GetLiteratureByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            // First get the Turkish name for the plant
            var turkishName = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT turkce FROM dbo.bitki WHERE bitkiid = @Id",
                new { Id = id }
            );

            if (string.IsNullOrEmpty(turkishName))
                return new List<PlantLiteratureDto>();

            var sql = @"
                SELECT DISTINCT
                    l.literaturid AS Id,
                    l.yazarad AS AuthorName,
                    l.arastirmaadi AS ResearchName,
                    l.kaynakadi AS SourceName,
                    l.yil AS Year,
                    l.tur AS Type
                FROM dbo.literatur l
                WHERE l.literaturid IN (
                    SELECT DISTINCT litno FROM dbo.etnobitkilit WHERE turkcead ILIKE '%' || @TurkishName || '%'
                    UNION
                    SELECT DISTINCT litno FROM dbo.aktivitebitkilit WHERE turkcead ILIKE '%' || @TurkishName || '%'
                )
                LIMIT 50";

            try
            {
                return await connection.QueryAsync<PlantLiteratureDto>(sql, new { TurkishName = turkishName });
            }
            catch
            {
                return new List<PlantLiteratureDto>();
            }
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

