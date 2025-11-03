using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryPatchDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(50, ErrorMessage = "El máximo permitido es 50 caracteres.")]
        [MinLength(5, ErrorMessage = "El mínimo permitido es 5 caracteres.")]
        public string? Name { get; set; }   // opcional

        [MaxLength(100, ErrorMessage = "El máximo permitido es 100 caracteres.")]
        [MinLength(10, ErrorMessage = "El mínimo permitido es 10 caracteres.")]
        public string? Description { get; set; }   // opcional
    }
}
