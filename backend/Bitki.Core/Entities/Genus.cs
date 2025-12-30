namespace Bitki.Core.Entities
{
    public class Genus
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int FamilyId { get; set; }
        public string? FamilyName { get; set; }
        public string? Description { get; set; }
    }
}
