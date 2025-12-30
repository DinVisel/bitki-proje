namespace Bitki.Blazor.Models
{
    public class AktiviteCalismaViewModel
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LocalityId { get; set; }
        public int? EffectId { get; set; }

        public string? LocalityName { get; set; }
        public string? EffectName { get; set; }
    }
}
