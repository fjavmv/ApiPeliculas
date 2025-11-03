using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El máximo permitido es 50 caracteres.")]
        [MinLength(5, ErrorMessage = "El mínimo permitido es 5 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(100, ErrorMessage = "El máximo permitido es 100 caracteres.")]
        [MinLength(10, ErrorMessage = "El mínimo permitido es 10 caracteres.")]
        public string Description { get; set; } = string.Empty;
    }
}
