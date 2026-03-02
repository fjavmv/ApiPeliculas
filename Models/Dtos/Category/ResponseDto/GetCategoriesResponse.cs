using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Category.ResponseDto
{
    public class GetCagoriesResponse
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; } = string.Empty;
    }
}
