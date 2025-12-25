namespace Bitki.Core.Entities
{
    public class Literatur
    {
        public long Id { get; set; }
        public string? AuthorName { get; set; }
        public string? ResearchName { get; set; }
        public string? SourceName { get; set; }
        public int? Year { get; set; }
        public string? FullName { get; set; }
        public string? Link { get; set; }
        public string? Type { get; set; }
        public string? TopicType { get; set; }
        public string? Reliability { get; set; }
        public string? Summary { get; set; }
    }
}
