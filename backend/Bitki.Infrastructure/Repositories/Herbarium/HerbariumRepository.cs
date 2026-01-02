
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

            // Map Entity Properties to DB Columns
            var allowedColumns = new[] { "herbariumid", "herbariumno", "bitkiid", "tarih", "sehirid", "ilceid", "koy", "rakim", "gps", "aciklama" };
            var searchableColumns = new[] { "herbariumno", "gps", "aciklama", "koy" };
            var columnMappings = new Dictionary<string, string>
            {
                { "Id", "herbariumid" },
                { "HerbariumNo", "herbariumno" },
                { "PlantId", "bitkiid" },
                { "CollectionDate", "tarih" },
                { "CityId", "sehirid" },
                { "DistrictId", "ilceid" },
                { "Village", "koy" },
                { "Altitude", "rakim" },
                { "Gps", "gps" },
                { "LocationDescription", "aciklama" }
            };
            _queryBuilder = new QueryBuilder("herbarium", allowedColumns, searchableColumns, columnMappings);
        }

        public async Task<Bitki.Core.Entities.Herbarium?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Main Record
            var sql = @"
                SELECT 
                    h.herbariumid AS Id, 
                    h.herbariumno AS HerbariumNo, 
                    h.bitkiid AS PlantId, 
                    b.turkce AS PlantName,
                    f.familyaadi AS FamilyName,
                    h.tarih AS CollectionDate, 
                    h.sehirid AS CityId, 
                    s.adi AS CityName,
                    h.ilceid AS DistrictId, 
                    i.adi AS DistrictName,
                    h.koy AS Village, 
                    h.rakim AS Altitude, 
                    h.gps AS Gps, 
                    h.aciklama AS LocationDescription, 
                    h.resimyolu AS ImagePath
                FROM dbo.herbarium h
                LEFT JOIN dbo.bitki b ON h.bitkiid = b.bitkiid
                LEFT JOIN dbo.familya f ON b.familyaid = f.familyaid
                LEFT JOIN dbo.sehir s ON h.sehirid = s.sehirid
                LEFT JOIN dbo.ilce i ON h.ilceid = i.ilceid
                WHERE h.herbariumid = @Id";

            var entity = await connection.QueryFirstOrDefaultAsync<Bitki.Core.Entities.Herbarium>(sql, new { Id = id });

            if (entity != null)
            {
                // Get Properties
                var propSql = "SELECT ozellikid FROM dbo.herbarium_ozellik WHERE herbariumid = @Id";
                var props = await connection.QueryAsync<long>(propSql, new { Id = id });

                entity.PropertyIds = props.ToList();

                // Get People
                var personSql = @"
                    SELECT hk.herbariumid, hk.kisiid AS PersonId, hk.gorev AS Role, k.adi AS PersonName
                    FROM dbo.herbarium_kisi hk
                    JOIN dbo.kisiler k ON hk.kisiid = k.kisiid
                    WHERE hk.herbariumid = @Id";
                var people = await connection.QueryAsync<HerbariumPerson>(personSql, new { Id = id });
                entity.People = people.ToList();
            }

            return entity;
        }

        public async Task<int> AddAsync(Bitki.Core.Entities.Herbarium entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var sql = @"INSERT INTO dbo.herbarium (herbariumno, bitkiid, tarih, sehirid, ilceid, koy, rakim, gps, aciklama, resimyolu) 
                            VALUES (@HerbariumNo, @PlantId, @CollectionDate, @CityId, @DistrictId, @Village, @Altitude, @Gps, @LocationDescription, @ImagePath) 
                            RETURNING herbariumid";

                var id = await connection.ExecuteScalarAsync<int>(sql, entity, transaction);

                if (entity.PropertyIds != null && entity.PropertyIds.Any())
                {
                    var propSql = "INSERT INTO dbo.herbarium_ozellik (herbariumid, ozellikid) VALUES (@Hid, @Pid)";
                    await connection.ExecuteAsync(propSql, entity.PropertyIds.Select(p => new { Hid = id, Pid = p }), transaction);
                }

                if (entity.People != null && entity.People.Any())
                {
                    var personSql = "INSERT INTO dbo.herbarium_kisi (herbariumid, kisiid, gorev) VALUES (@Hid, @Pid, @Role)";
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
                var sql = @"UPDATE dbo.herbarium 
                            SET herbariumno = @HerbariumNo, 
                                bitkiid = @PlantId, 
                                tarih = @CollectionDate, 
                                sehirid = @CityId, 
                                ilceid = @DistrictId, 
                                koy = @Village, 
                                rakim = @Altitude, 
                                gps = @Gps, 
                                aciklama = @LocationDescription, 
                                resimyolu = @ImagePath
                            WHERE herbariumid = @Id";

                await connection.ExecuteAsync(sql, entity, transaction);

                // Update Properties (Delete all and re-insert)
                await connection.ExecuteAsync("DELETE FROM dbo.herbarium_ozellik WHERE herbariumid = @Id", new { Id = entity.Id }, transaction);
                if (entity.PropertyIds != null && entity.PropertyIds.Any())
                {
                    var propSql = "INSERT INTO dbo.herbarium_ozellik (herbariumid, ozellikid) VALUES (@Hid, @Pid)";
                    await connection.ExecuteAsync(propSql, entity.PropertyIds.Select(p => new { Hid = entity.Id, Pid = p }), transaction);
                }

                // Update People
                await connection.ExecuteAsync("DELETE FROM dbo.herbarium_kisi WHERE herbariumid = @Id", new { Id = entity.Id }, transaction);
                if (entity.People != null && entity.People.Any())
                {
                    var personSql = "INSERT INTO dbo.herbarium_kisi (herbariumid, kisiid, gorev) VALUES (@Hid, @Pid, @Role)";
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
                await connection.ExecuteAsync("DELETE FROM dbo.herbarium_ozellik WHERE herbariumid = @Id", new { Id = id }, transaction);
                await connection.ExecuteAsync("DELETE FROM dbo.herbarium_kisi WHERE herbariumid = @Id", new { Id = id }, transaction);
                await connection.ExecuteAsync("DELETE FROM dbo.herbarium WHERE herbariumid = @Id", new { Id = id }, transaction);
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
            var parameters = new DynamicParameters();

            // Simplified Select for List
            var selectColumns = "h.herbariumid AS Id, h.herbariumno AS HerbariumNo, b.turkce AS PlantName, h.tarih AS CollectionDate, s.adi AS CityName";

            // Note: QueryBuilder might not handle Joins easily for dynamic filtering if mappings don't match exactly.
            // But custom sql works.
            // Using a view or just simple join in From clause.
            // Overriding BuildSelectQuery logic slightly by manual construction or assuming names match.
            // Here I'll use simple select.

            var baseSql = "FROM dbo.herbarium h LEFT JOIN dbo.bitki b ON h.bitkiid = b.bitkiid LEFT JOIN dbo.sehir s ON h.sehirid = s.sehirid";

            // Ideally modify QueryBuilder to support Joins, or use simple query. 
            // For now, I'll stick to basic implementation and might improve later if List Page is requested.
            // The Task is "Add/Edit Screen", so List Page is not primary focus here, but QueryAsync is part of interface.

            // Just return empty for now or basic implementation?
            // I'll implement basic.

            var sql = $"SELECT {selectColumns} {baseSql} ORDER BY h.herbariumid DESC LIMIT @PageSize OFFSET @Offset";
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);

            var data = await connection.QueryAsync<Bitki.Core.Entities.Herbarium>(sql, parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.herbarium");

            return new FilterResponse<Bitki.Core.Entities.Herbarium>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
