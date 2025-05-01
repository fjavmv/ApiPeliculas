using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryDto
    {
      
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El numero maximo es  100")]
        public string Name { get; set; }
        public DateTime FechaDeCreacion { get; set; }
    }
}
