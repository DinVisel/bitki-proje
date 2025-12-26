namespace Bitki.Core.Entities
{
    /// <summary>
    /// Full representation of the Bitki table with all columns for detail view
    /// </summary>
    public class PlantDetail
    {
        // Primary Key
        public int Id { get; set; }

        // Basic Information
        public string? TurkishName { get; set; }
        public string? LatinName { get; set; }
        public string? Description { get; set; }

        // Taxonomy Foreign Keys
        public int? FamilyId { get; set; }
        public int? GenusId { get; set; }

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

        // Additional String Fields
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
        public string? Endemism { get; set; }
        public string? EndemismDescription { get; set; }
        public string? FloweringTime { get; set; }
        public string? Habitat { get; set; }
        public string? Altitude { get; set; }
        public string? Distribution { get; set; }
        public string? Phytogeography { get; set; }
        public string? CommonNames { get; set; }
        public string? Notes { get; set; }
        public string? References { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
