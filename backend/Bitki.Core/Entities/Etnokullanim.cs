namespace Bitki.Core.Entities
{
    public class Etnokullanim
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public int LocalityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LocalityName { get; set; }
    }
}
