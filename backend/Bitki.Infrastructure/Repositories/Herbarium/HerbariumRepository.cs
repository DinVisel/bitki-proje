
using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Herbarium;
using Bitki.Core.Models;
using Bitki.Core.Utilities;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Herbarium
{
    public class HerbariumRepository : IHerbariumRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly QueryBuilder _queryBuilder;

        public HerbariumRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // CORRECT COLUMN NAMES (discovered via SELECT *)
            // hkid = PK, kod = herbarium number, bitkino = plant FK
            // sehirno = city FK, tarih = date
            var allowedColumns = new[] {
                "hkid", "kod", "bitkino", "tarih", "sehirno", "ilce", "koy",
                "h.hkid", "h.kod", "h.bitkino", "h.tarih", "h.sehirno", "h.ilce", "h.koy",
                "b.turkce", "b.bitkiid", "plantname", "f.familya", "s.adi"
            };
            var searchableColumns = new[] { "h.kod", "b.turkce", "h.koy", "h.ilce" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "h.hkid" },
                { "HerbariumNo", "h.kod" },
                { "PlantId", "h.bitkino" },
                { "PlantName", "b.turkce" },
                { "FamilyName", "f.familya" },
                { "CollectionDate", "h.tarih" },
                { "CityId", "h.sehirno" },
                { "Village", "h.koy" }
            };
            _queryBuilder = new QueryBuilder("herbaryum", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<Bitki.Core.Entities.Herbarium?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                // Use SELECT * to discover columns dynamically
                var sql = "SELECT * FROM dbo.herbaryum LIMIT 1";
                var sample = await connection.QueryFirstOrDefaultAsync<dynamic>(sql);

                if (sample == null)
                    return null;

                // Discover the primary key column name
                var dict = (IDictionary<string, object>)sample;
                var pkColumn = dict.Keys.FirstOrDefault(k => k.ToLower().Contains("id") || k.ToLower() == "no") ?? "id";

                // Query by the discovered primary key
                var fetchSql = $"SELECT * FROM dbo.herbaryum WHERE {pkColumn} = @Id";
                var row = await connection.QueryFirstOrDefaultAsync<dynamic>(fetchSql, new { Id = id });

                if (row == null)
                    return null;

                var rowDict = (IDictionary<string, object>)row;
                var entity = new Bitki.Core.Entities.Herbarium
                {
                    Id = rowDict.ContainsKey("hkid") ? Convert.ToInt32(rowDict["hkid"] ?? 0) : Convert.ToInt32(rowDict.FirstOrDefault(x => x.Key.ToLower().Contains("id") || x.Key.ToLower() == "no").Value ?? 0),
                    HerbariumNo = rowDict.ContainsKey("kod") ? rowDict["kod"]?.ToString() : rowDict.FirstOrDefault(x => x.Key.ToLower().Contains("no") && !x.Key.ToLower().Contains("bitkino")).Value?.ToString(),
                    PlantId = rowDict.ContainsKey("bitkino") ? Convert.ToInt32(rowDict["bitkino"] ?? 0) : (rowDict.ContainsKey("bitkiid") ? Convert.ToInt32(rowDict["bitkiid"] ?? 0) : 0),
                    CollectionDate = rowDict.ContainsKey("tarih") ? (DateTime?)rowDict["tarih"] : null
                };

                // Log discovered columns for debugging
                Console.WriteLine($"Herbaryum columns: {string.Join(", ", rowDict.Keys)}");

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Herbarium GetByIdAsync Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> AddAsync(Bitki.Core.Entities.Herbarium entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var sql = @"INSERT INTO dbo.herbaryum (kod, bitkino, tarih) 
                            VALUES (@HerbariumNo, @PlantId, @CollectionDate) 
                            RETURNING hkid";

                var id = await connection.ExecuteScalarAsync<int>(sql, entity, transaction);

                if (entity.PropertyIds != null && entity.PropertyIds.Any())
                {
                    var propSql = "INSERT INTO dbo.herbaryum_ozellik (hkid, ozellikid) VALUES (@Hid, @Pid)";
                    await connection.ExecuteAsync(propSql, entity.PropertyIds.Select(p => new { Hid = id, Pid = p }), transaction);
                }

                if (entity.People != null && entity.People.Any())
                {
                    var personSql = "INSERT INTO dbo.herbaryum_kisi (hkid, kisiid, gorev) VALUES (@Hid, @Pid, @Role)";
                    await connection.ExecuteAsync(personSql, entity.People.Select(p => new { Hid = id, Pid = p.PersonId, Role = p.Role }), transaction);
                }

                transaction.Commit();
                return id;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task UpdateAsync(Bitki.Core.Entities.Herbarium entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var sql = @"UPDATE dbo.herbaryum 
                            SET kod = @HerbariumNo, 
                                bitkino = @PlantId, 
                                tarih = @CollectionDate, 
                                sehirno = @CityId, 
                                koy = @Village
                            WHERE hkid = @Id";

                await connection.ExecuteAsync(sql, entity, transaction);

                // Update Properties (Delete all and re-insert)
                await connection.ExecuteAsync("DELETE FROM dbo.herbaryum_ozellik WHERE hkid = @Id", new { Id = entity.Id }, transaction);
                if (entity.PropertyIds != null && entity.PropertyIds.Any())
                {
                    var propSql = "INSERT INTO dbo.herbaryum_ozellik (hkid, ozellikid) VALUES (@Hid, @Pid)";
                    await connection.ExecuteAsync(propSql, entity.PropertyIds.Select(p => new { Hid = entity.Id, Pid = p }), transaction);
                }

                // Update People
                await connection.ExecuteAsync("DELETE FROM dbo.herbaryum_kisi WHERE hkid = @Id", new { Id = entity.Id }, transaction);
                if (entity.People != null && entity.People.Any())
                {
                    var personSql = "INSERT INTO dbo.herbaryum_kisi (hkid, kisiid, gorev) VALUES (@Hid, @Pid, @Role)";
                    await connection.ExecuteAsync(personSql, entity.People.Select(p => new { Hid = entity.Id, Pid = p.PersonId, Role = p.Role }), transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            // Relations should be cascade deleted by DB or we delete them manually
            // Assuming Manual needed if no cascade:
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync("DELETE FROM dbo.herbaryum_ozellik WHERE hkid = @Id", new { Id = id }, transaction);
                await connection.ExecuteAsync("DELETE FROM dbo.herbaryum_kisi WHERE hkid = @Id", new { Id = id }, transaction);
                await connection.ExecuteAsync("DELETE FROM dbo.herbaryum WHERE hkid = @Id", new { Id = id }, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<FilterResponse<Bitki.Core.Entities.Herbarium>> QueryAsync(FilterRequest request)
        {
            request.ValidatePagination();

            using var connection = _connectionFactory.CreateConnection();

            try
            {
                // Minimal query to discover what columns exist
                // Avoiding all assumptions about column names
                var sql = @"
                    SELECT * FROM dbo.herbaryum 
                    ORDER BY 1 DESC
                    LIMIT @PageSize OFFSET @Offset";

                var offset = (request.PageNumber - 1) * request.PageSize;
                var data = await connection.QueryAsync<dynamic>(sql, new { PageSize = request.PageSize, Offset = offset });

                // Map dynamic results to entity - try common column name patterns
                var entities = data.Select(row =>
                {
                    var dict = (IDictionary<string, object>)row;
                    return new Bitki.Core.Entities.Herbarium
                    {
                        Id = dict.ContainsKey("hkid") ? Convert.ToInt32(dict["hkid"] ?? 0) : Convert.ToInt32(dict.FirstOrDefault(x => x.Key.ToLower().Contains("id") || x.Key.ToLower() == "no").Value ?? 0),
                        HerbariumNo = dict.ContainsKey("kod") ? dict["kod"]?.ToString() : dict.FirstOrDefault(x => x.Key.ToLower().Contains("no") && !x.Key.ToLower().Contains("bitkino")).Value?.ToString(),
                        PlantId = dict.ContainsKey("bitkino") ? Convert.ToInt32(dict["bitkino"] ?? 0) : (dict.ContainsKey("bitkiid") ? Convert.ToInt32(dict["bitkiid"] ?? 0) : 0),
                        CollectionDate = dict.ContainsKey("tarih") ? (DateTime?)dict["tarih"] : null
                    };
                }).ToList();

                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.herbaryum");

                return new FilterResponse<Bitki.Core.Entities.Herbarium>
                {
                    Data = entities,
                    TotalCount = totalCount,
                    FilteredCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Herbarium QueryAsync Error: {ex.Message}");
                // Return empty result on error
                return new FilterResponse<Bitki.Core.Entities.Herbarium>
                {
                    Data = new List<Bitki.Core.Entities.Herbarium>(),
                    TotalCount = 0,
                    FilteredCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
        }
    }
}
