using System.Text;
using Dapper;

namespace Bitki.Core.Utilities
{
    /// <summary>
    /// Utility class for building safe, parameterized SQL queries with Dapper
    /// </summary>
    public class QueryBuilder
    {
        private readonly HashSet<string> _allowedColumns;
        private readonly HashSet<string> _searchableColumns;
        private readonly string _tableName;
        private readonly string _schema;
        private readonly bool _hasSoftDelete;

        public QueryBuilder(string tableName, IEnumerable<string> allowedColumns, IEnumerable<string> searchableColumns, string schema = "dbo", bool hasSoftDelete = false)
        {
            _tableName = tableName;
            _schema = schema;
            _hasSoftDelete = hasSoftDelete;
            _allowedColumns = new HashSet<string>(allowedColumns, StringComparer.OrdinalIgnoreCase);
            _searchableColumns = new HashSet<string>(searchableColumns, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates if a column name is in the allowed list
        /// </summary>
        public bool IsColumnAllowed(string columnName)
        {
            return _allowedColumns.Contains(columnName);
        }

        /// <summary>
        /// Validates if a column is searchable
        /// </summary>
        public bool IsColumnSearchable(string columnName)
        {
            return _searchableColumns.Contains(columnName);
        }

        /// <summary>
        /// Builds a WHERE clause for search text across searchable columns
        /// </summary>
        public string BuildSearchClause(string? searchText, DynamicParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return string.Empty;

            var conditions = new List<string>();
            foreach (var column in _searchableColumns)
            {
                conditions.Add($"{column} ILIKE @SearchPattern");
            }

            parameters.Add("SearchPattern", $"%{searchText}%");
            return conditions.Count > 0 ? $"({string.Join(" OR ", conditions)})" : string.Empty;
        }

        /// <summary>
        /// Builds WHERE clauses for column-specific filters
        /// </summary>
        public List<string> BuildFilterClauses(Dictionary<string, string>? filters, DynamicParameters parameters)
        {
            var clauses = new List<string>();
            if (filters == null || filters.Count == 0)
                return clauses;

            int paramIndex = 0;
            foreach (var filter in filters)
            {
                if (!IsColumnAllowed(filter.Key))
                    continue; // Skip invalid columns

                var paramName = $"Filter{paramIndex}";
                clauses.Add($"{filter.Key} = @{paramName}");
                parameters.Add(paramName, filter.Value);
                paramIndex++;
            }

            return clauses;
        }

        /// <summary>
        /// Builds an ORDER BY clause with validation
        /// </summary>
        public string BuildOrderByClause(string? sortColumn, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortColumn) || !IsColumnAllowed(sortColumn))
                return string.Empty;

            // Validate sort direction
            var direction = sortDirection?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";
            return $"ORDER BY {sortColumn} {direction}";
        }

        /// <summary>
        /// Builds a complete SELECT query with filters, search, and sorting
        /// </summary>
        public string BuildSelectQuery(
            string selectColumns,
            string? searchText,
            Dictionary<string, string>? filters,
            string? sortColumn,
            string sortDirection,
            DynamicParameters parameters,
            bool includeDeleted = false)
        {
            var sql = new StringBuilder();
            sql.AppendLine($"SELECT {selectColumns}");
            sql.AppendLine($"FROM {_schema}.{_tableName}");

            var whereClauses = new List<string>();

            // Add soft delete filter if applicable
            if (_hasSoftDelete && !includeDeleted)
            {
                whereClauses.Add("IsDeleted = false");
            }

            // Add search clause
            var searchClause = BuildSearchClause(searchText, parameters);
            if (!string.IsNullOrEmpty(searchClause))
            {
                whereClauses.Add(searchClause);
            }

            // Add filter clauses
            var filterClauses = BuildFilterClauses(filters, parameters);
            whereClauses.AddRange(filterClauses);

            // Build WHERE clause
            if (whereClauses.Count > 0)
            {
                sql.AppendLine($"WHERE {string.Join(" AND ", whereClauses)}");
            }

            // Add ORDER BY
            var orderBy = BuildOrderByClause(sortColumn, sortDirection);
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql.AppendLine(orderBy);
            }

            return sql.ToString();
        }

        /// <summary>
        /// Builds a COUNT query with the same filters
        /// </summary>
        public string BuildCountQuery(
            string? searchText,
            Dictionary<string, string>? filters,
            DynamicParameters parameters,
            bool includeDeleted = false)
        {
            var sql = new StringBuilder();
            sql.AppendLine($"SELECT COUNT(*)");
            sql.AppendLine($"FROM {_schema}.{_tableName}");

            var whereClauses = new List<string>();

            // Add soft delete filter if applicable
            if (_hasSoftDelete && !includeDeleted)
            {
                whereClauses.Add("IsDeleted = false");
            }

            // Add search clause
            var searchClause = BuildSearchClause(searchText, parameters);
            if (!string.IsNullOrEmpty(searchClause))
            {
                whereClauses.Add(searchClause);
            }

            // Add filter clauses
            var filterClauses = BuildFilterClauses(filters, parameters);
            whereClauses.AddRange(filterClauses);

            // Build WHERE clause
            if (whereClauses.Count > 0)
            {
                sql.AppendLine($"WHERE {string.Join(" AND ", whereClauses)}");
            }

            return sql.ToString();
        }
    }
}
