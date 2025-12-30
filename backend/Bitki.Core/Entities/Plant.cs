using System;

namespace Bitki.Core.Entities
{
    public class Plant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LatinName { get; set; }
        public int? GenusId { get; set; }
        public string? Description { get; set; }

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
        public string? DistributionTurkey { get; set; } // tdagilim
        public string? DistributionWorld { get; set; } // ddagilim
        public string? Phytogeography { get; set; } // davis

        // Taxonomy & Other
        public string? CommonNames { get; set; } // sinonimler
        public string? TaxonName { get; set; } // species
        public string? TaxonKind { get; set; } // subspecies (or kind?)

        // Navigation properties (optional/for display)
        public string? FamilyName { get; set; }
        public string? GenusName { get; set; }
    }
}
