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
        /// Current page number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 20;

        // Computed properties for UI
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)FilteredCount / PageSize) : 0;
        public int StartRecord => FilteredCount > 0 ? (PageNumber - 1) * PageSize + 1 : 0;
        public int EndRecord => Math.Min(PageNumber * PageSize, FilteredCount);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
