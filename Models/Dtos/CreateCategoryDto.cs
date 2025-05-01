using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CreateCategoryDto
    {
      
      
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El numero maximo es  100")]
        public string Name { get; set; }
      
    }
}
