using ApiImages.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FileName { get; set; } = string.Empty; // nombre original o amigable

        [Required]
        [MaxLength(300)]
        public string Path { get; set; } = string.Empty; // ruta o URL de acceso

        [MaxLength(50)]
        public string ContentType { get; set; } = string.Empty; // ej. "image/jpeg"

        public long Size { get; set; } // en bytes

        public DateTime CreationDate { get; set; }

        public DateTime? LastUpdate { get; set; }

        public bool IsActive { get; set; } = true; // borrado lógico

        // Relación muchos a muchos
        public ICollection<Category> Categories { get; set; } = new List<Category>();


    }
}
