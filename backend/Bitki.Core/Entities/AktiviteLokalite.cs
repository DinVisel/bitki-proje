namespace Bitki.Core.Entities
{
    public class AktiviteLokalite
    {
        public long Id { get; set; }
        public string? LocalName { get; set; }
        public string? Location { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }

        // Relational properties
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
    }
}
