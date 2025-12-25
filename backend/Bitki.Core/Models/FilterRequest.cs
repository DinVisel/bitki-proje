namespace Bitki.Core.Models
{
    /// <summary>
    /// Generic filter request model for server-side filtering, sorting, and pagination
    /// </summary>
    public class FilterRequest
    {
        /// <summary>
        /// Global search text - searches across all searchable columns using ILIKE
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Column-specific filters for exact matches
        /// Key: column name, Value: filter value
        /// </summary>
        public Dictionary<string, string>? Filters { get; set; }

        /// <summary>
        /// Column name to sort by (must be validated against whitelist)
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Sort direction: "ASC" or "DESC"
        /// </summary>
        public string SortDirection { get; set; } = "ASC";

        /// <summary>
        /// Page number for pagination (1-indexed)
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size for pagination (default 20, max 100)
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Include soft-deleted records (admin only)
        /// </summary>
        public bool IncludeDeleted { get; set; } = false;

        /// <summary>
        /// Validates and corrects pagination parameters
        /// </summary>
        public void ValidatePagination()
        {
            if (PageNumber < 1) PageNumber = 1;
            if (PageSize < 1) PageSize = 20;
            if (PageSize > 100) PageSize = 100;
        }
    }
}
