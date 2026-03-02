using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Image.ResponseDto
{
    public class GetImageResponse
    {
        [Required(ErrorMessage = "El nombre del archivo es obligatorio.")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL es obligatoria.")]
        public string Path { get; set; } = string.Empty;

        // Opcional: asociar imágenes a categorías existentes
        #nullable enable
        public ICollection<Guid>? CategoryIds { get; set; }
    }

}
