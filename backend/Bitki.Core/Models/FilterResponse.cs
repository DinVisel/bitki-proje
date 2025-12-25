namespace Bitki.Core.Models
{
    /// <summary>
    /// Generic filter response model containing filtered data and metadata
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class FilterResponse<T>
    {
        /// <summary>
        /// Filtered and sorted records
        /// </summary>
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Total number of records before filtering
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of records after filtering
        /// </summary>
        public int FilteredCount { get; set; }

        /// <summary>
        /// Current page number (if pagination is applied)
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Page size (if pagination is applied)
        /// </summary>
        public int? PageSize { get; set; }
    }
}
