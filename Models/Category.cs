using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiPeliculas.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength (50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true; // por defecto activo

        public DateTime CreationDate { get; set; }
        
        public DateTime? LastUpdate { get; set; }

    }
}
