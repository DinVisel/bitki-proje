using System.ComponentModel.DataAnnotations;

namespace Bitki.Blazor.Models
{
    /// <summary>
    /// Model for editing a generic Bitki record.
    /// Maps to the core Plant entity but optimized for Blazor forms.
    /// </summary>
    public class BitkiEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Türkçe Ad zorunludur")]
        public string? Name { get; set; } // maps to TurkishName

        public string? LatinName { get; set; }
        public string? Description { get; set; }

        // Taxonomy
        public int? GenusId { get; set; }
        // Family is derived from Genus usually, but we might want to show it or allow filtering genera by it? 
        // For now, we edit GenusId directly.

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

        // Ecology & Distribution
        public string? Endemism { get; set; }
        public string? EndemismDescription { get; set; }
        public int? FirstFloweringTime { get; set; }
        public int? LastFloweringTime { get; set; }
        public string? Habitat { get; set; }
        public int? MinAltitude { get; set; }
        public int? MaxAltitude { get; set; }
        public string? DistributionTurkey { get; set; }
        public string? DistributionWorld { get; set; }
        public string? Phytogeography { get; set; }

        // Taxonomy & Other
        public string? CommonNames { get; set; }
        public string? TaxonName { get; set; }
        public string? TaxonKind { get; set; }
    }
}
