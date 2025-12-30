namespace Bitki.Blazor.Models
{
    public class AktiviteBitkilitViewModel
    {
        public long Id { get; set; }
        public string? TurkishName { get; set; }
        public string? Status { get; set; }
        public int LiteratureId { get; set; }
        public int? FamilyId { get; set; }
        public int? GenusId { get; set; }

        public string? LiteratureName { get; set; }
        public string? FamilyName { get; set; }
        public string? GenusName { get; set; }
    }
}
