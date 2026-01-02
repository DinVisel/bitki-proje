
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

        // Additional Herbarium Card Fields
        public string? Tipus { get; set; }              // Type/Category
        public string? ItfKaresi { get; set; }          // ITF Grid Square
        public string? Substrat { get; set; }           // Substrate
        public string? Habitat { get; set; }            // Habitat description
        public decimal? Sicaklik { get; set; }          // Temperature
        public decimal? Nem { get; set; }               // Humidity
        public string? Yogunluk { get; set; }           // Density
        public int? OrnekSayisi { get; set; }           // Sample Count
        public string? GeldigiHerbaryum { get; set; }   // Source Herbarium
        public string? GittigiHerbaryum { get; set; }   // Destination Herbarium
        public string? CicekRengi { get; set; }         // Flower Color
        public bool EksikTeshis { get; set; }           // Incomplete Identification
        public int? Yukseklik { get; set; }             // Height/Elevation
        public string? Yer { get; set; }                // Locality (detailed location)

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
