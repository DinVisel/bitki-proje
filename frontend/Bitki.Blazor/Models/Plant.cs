using System.ComponentModel.DataAnnotations;

namespace Bitki.Blazor.Models
{
    public class Plant
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? LatinName { get; set; }

        public string? Family { get; set; }

        // API returns FamilyName and GenusName from JOIN
        public string? FamilyName { get; set; }
        public string? GenusName { get; set; }

        public int? GenusId { get; set; }

        public string? Habitat { get; set; }

        public string? Description { get; set; }

        // Computed display name - use actual name or fallback to latin/id
        public string DisplayName => !string.IsNullOrEmpty(Name) ? Name
            : !string.IsNullOrEmpty(LatinName) ? LatinName
            : $"(İsimsiz Bitki #{Id})";

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
