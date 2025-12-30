namespace Bitki.Blazor.Models
{
    public class GenusViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FamilyId { get; set; }
        public string? FamilyName { get; set; }
        public string? Description { get; set; }
    }
}
