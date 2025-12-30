namespace Bitki.Blazor.Models
{
    public class LiteraturBilesikViewModel
    {
        public long Id { get; set; }
        public int? LiteratureId { get; set; }
        public int? CompoundId { get; set; }
        public string? Description { get; set; }
        public string? LiteratureName { get; set; }
        public string? CompoundName { get; set; }
    }
}
