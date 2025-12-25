using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Interfaces;
using Dapper;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SchemaController(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet("tables")]
        public async Task<IActionResult> GetTables()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT table_schema, table_name 
                FROM information_schema.tables 
                WHERE table_type = 'BASE TABLE' 
                AND table_schema NOT IN ('information_schema', 'pg_catalog')
                ORDER BY table_schema, table_name";

            var tables = await connection.QueryAsync(sql);
            return Ok(tables);
        }

        [HttpGet("columns/{tableName}")]
        public async Task<IActionResult> GetColumns(string tableName)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT column_name, data_type, is_nullable
                FROM information_schema.columns 
                WHERE table_name = @TableName
                ORDER BY ordinal_position";

            var columns = await connection.QueryAsync(sql, new { TableName = tableName });
            return Ok(columns);
        }
    }
}
