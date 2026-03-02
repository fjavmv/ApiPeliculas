using ApiImages.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiImages.Models
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

        // Relación muchos a muchos
        public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
