namespace Bitki.Core.Entities
{
    public class Ilce
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int CityId { get; set; }
        public string? CityName { get; set; }
    }
}
