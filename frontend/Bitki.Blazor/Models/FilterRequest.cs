namespace Bitki.Blazor.Models
{
    /// <summary>
    /// Filter request model for server-side filtering
    /// </summary>
    public class FilterRequest
    {
        public string? SearchText { get; set; }
        public Dictionary<string, string>? Filters { get; set; }
        public string? SortColumn { get; set; }
        public string SortDirection { get; set; } = "ASC";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool IncludeDeleted { get; set; } = false;
    }
}
