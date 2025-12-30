namespace Bitki.Core.DTOs
{
    /// <summary>
    /// DTO for a compound related to a plant with resolved names
    /// </summary>
    public class PlantCompoundDto
    {
        public long Id { get; set; }
        public int CompoundId { get; set; }
        public string? CompoundName { get; set; }
        public string? CompoundEnglishName { get; set; }
        public string? CompoundLatinName { get; set; }
        public float? Amount { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for plant image
    /// </summary>
    public class PlantImageDto
    {
        public long Id { get; set; }
        public string? ImageLocation { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for related literature record
    /// </summary>
    public class PlantLiteratureDto
    {
        public long Id { get; set; }
        public string? AuthorName { get; set; }
        public string? ResearchName { get; set; }
        public string? SourceName { get; set; }
        public int? Year { get; set; }
        public string? Type { get; set; }
    }

    /// <summary>
    /// Full detail DTO for Bitki including all related data
    /// </summary>
    public class BitkiDetailDto
    {
        // Basic Information
        public int Id { get; set; }
        public string? TurkishName { get; set; }
        public string? LatinName { get; set; }
        public string? Description { get; set; }

        // Taxonomy - Resolved Names
        public int? FamilyId { get; set; }
        public string? FamilyName { get; set; }
        public string? FamilyTurkishName { get; set; }
        public int? GenusId { get; set; }
        public string? GenusName { get; set; }

        // Boolean Flags
        public bool IsMedicinal { get; set; }
        public bool IsFood { get; set; }
        public bool IsCultural { get; set; }
        public bool IsPoisonous { get; set; }
        public bool IsTurkishFlora { get; set; }
        public bool IsIslandSpecies { get; set; }
        public bool ExistenceDoubtful { get; set; }
        public bool NeedsRevision { get; set; }
        public bool IsExtinct { get; set; }
        public bool IncompleteIdentification { get; set; }
        public bool ControlOk { get; set; }
        public bool PublicationOk { get; set; }

        // Taxon Information
        public string? Otor { get; set; }
        public string? BasionymOtor { get; set; }
        public string? Year { get; set; }
        public string? Pages { get; set; }
        public string? TaxonName { get; set; }
        public string? TaxonOtor { get; set; }
        public string? TaxonLevel { get; set; }
        public string? TaxonKind { get; set; }
        public string? Synonym { get; set; }
        public string? SynonymOtor { get; set; }
        public string? Status { get; set; }

        // Distribution & Ecology
        public string? Endemism { get; set; }
        public string? EndemismDescription { get; set; }
        public string? FloweringTime { get; set; }
        public int? FirstFloweringTime { get; set; }
        public int? LastFloweringTime { get; set; }
        public string? Habitat { get; set; }
        public string? Altitude { get; set; }
        public int? MinAltitude { get; set; }
        public int? MaxAltitude { get; set; }
        public string? Distribution { get; set; }
        public string? DistributionTurkey { get; set; }
        public string? DistributionWorld { get; set; }
        public string? Phytogeography { get; set; }

        // Other Information
        public string? CommonNames { get; set; }
        public string? Notes { get; set; }
        public string? References { get; set; }
        public string? AdditionalInfo { get; set; }

        // Related Data Collections
        public List<PlantCompoundDto> Compounds { get; set; } = new();
        public List<PlantImageDto> Images { get; set; } = new();
        public List<PlantLiteratureDto> Literature { get; set; } = new();
    }
}
