namespace Bitki.Core.Entities
{
    public class AktiviteCalisma
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LocalityId { get; set; }
        public int? EffectId { get; set; }
    }
}
