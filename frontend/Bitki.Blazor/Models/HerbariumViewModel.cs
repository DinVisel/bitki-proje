
namespace Bitki.Blazor.Models
{
    public class HerbariumViewModel
    {
        public int Id { get; set; }
        public string? HerbariumNo { get; set; }
        public int? PlantId { get; set; }
        public DateTime? CollectionDate { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string? Village { get; set; }
        public int? Altitude { get; set; }
        public string? Gps { get; set; }
        public string? LocationDescription { get; set; }
        public string? ImagePath { get; set; }

        // Navigation properties (optional, for display)
        public string? PlantName { get; set; }
        public string? FamilyName { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }

        public List<long> PropertyIds { get; set; } = new();
        public List<HerbariumPersonViewModel> People { get; set; } = new();
    }

    public class HerbariumPersonViewModel
    {
        public int HerbariumId { get; set; }
        public long PersonId { get; set; }
        public string Role { get; set; } = string.Empty; // Collector, Identifier

        // Display
        public string? PersonName { get; set; }
    }
}
