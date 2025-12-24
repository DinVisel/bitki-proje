using System.ComponentModel.DataAnnotations;

namespace Bitki.Blazor.Models
{
    public class Plant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string LatinName { get; set; }

        public string Family { get; set; }

        public string Habitat { get; set; }

        public string Description { get; set; }
    }
}
