using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class UpdateCategoryDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El numero maximo es  100")]
        [MinLength(10, ErrorMessage = "El numero minimo es 10")]
        public string Name { get; set; }
       
    }
}
