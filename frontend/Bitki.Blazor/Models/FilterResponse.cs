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
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
