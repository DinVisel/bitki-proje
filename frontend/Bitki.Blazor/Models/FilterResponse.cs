namespace Bitki.Blazor.Models
{
    /// <summary>
    /// Filter response model containing filtered data and metadata
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class FilterResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        public int StartRecord { get; set; }
        public int EndRecord { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
