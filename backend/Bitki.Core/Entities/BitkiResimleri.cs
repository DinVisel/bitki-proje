namespace Bitki.Core.Entities
{
    public class BitkiResimleri
    {
        public long Id { get; set; }
        public int PlantId { get; set; }
        public string? ImageLocation { get; set; }
        public string? Description { get; set; }
    }
}
