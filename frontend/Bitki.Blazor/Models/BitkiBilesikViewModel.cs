namespace Bitki.Blazor.Models
{
    public class BitkiBilesikViewModel
    {
        public long Id { get; set; }
        public int PlantId { get; set; }
        public int CompoundId { get; set; }
        public float? Amount { get; set; }
        public string? Description { get; set; }
        public string? PlantName { get; set; }
        public string? CompoundName { get; set; }
    }
}
