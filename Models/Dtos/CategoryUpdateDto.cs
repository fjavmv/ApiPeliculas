using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryUpdateDto : CategoryCreateDto
    {
        [Required]
        public Guid Id { get; set; }

    }
}
