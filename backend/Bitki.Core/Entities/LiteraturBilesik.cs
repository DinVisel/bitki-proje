namespace Bitki.Core.Entities
{
    public class LiteraturBilesik
    {
        public long Id { get; set; }
        public int? LiteratureId { get; set; }
        public int? CompoundId { get; set; }
        public string? Description { get; set; }
    }
}
