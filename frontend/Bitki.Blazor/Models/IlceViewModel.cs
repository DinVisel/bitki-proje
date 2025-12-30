namespace Bitki.Blazor.Models
{
    public class IlceViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string? CityName { get; set; }
    }
}
