namespace Bitki.Blazor.Models
{
    /// <summary>
    /// Comprehensive ViewModel for Bitki list and Add/Edit operations
    /// </summary>
    public class BitkiViewModel
    {
        // Basic Information
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Maps to TurkishName
        public string LatinName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Habitat { get; set; } = string.Empty;

        // Extended fields for full Add/Edit
        public int? FamilyId { get; set; }
        public int? GenusId { get; set; }
        public string? GenusName { get; set; }
        public string? CommonNames { get; set; }
        public string? Status { get; set; }

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

        // Distribution & Ecology
        public string? Endemism { get; set; }
        public string? EndemismDescription { get; set; }
        public string? FloweringTime { get; set; }
        public string? Altitude { get; set; }
        public string? Distribution { get; set; }
        public string? Phytogeography { get; set; }

        // Other Information
        public string? Notes { get; set; }
        public string? References { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}

