
using Dapper;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Ozellik;

namespace Bitki.Infrastructure.Repositories.Ozellik
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PropertyTypeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<PropertyType>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                // Use SELECT * to discover column names
                var sql = "SELECT * FROM dbo.ozelliktipleri ORDER BY 2";
                var data = await connection.QueryAsync<dynamic>(sql);

                var results = data.Select(row =>
                {
                    var dict = (IDictionary<string, object>)row;

                    // Log discovered columns on first row
                    Console.WriteLine($"OzellikTipleri columns: {string.Join(", ", dict.Keys)}");

                    // Find ID column (first column containing 'id' or 'no')
                    var idKey = dict.Keys.FirstOrDefault(k => k.ToLower().Contains("id") || k.ToLower().EndsWith("no")) ?? dict.Keys.First();
                    // Find Name column (column containing 'ad' or 'name')
                    var nameKey = dict.Keys.FirstOrDefault(k => k.ToLower().Contains("ad") || k.ToLower().Contains("name")) ?? dict.Keys.Skip(1).First();

                    return new PropertyType
                    {
                        Id = Convert.ToInt32(dict[idKey] ?? 0),
                        Name = dict[nameKey]?.ToString() ?? ""
                    };
                }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PropertyType GetAllAsync Error: {ex.Message}");
                return new List<PropertyType>();
            }
        }
    }
}
