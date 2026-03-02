using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Image.RequestDto
{
    public class CreateImageRequest
    {
        [Required(ErrorMessage = "El nombre del archivo es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El máximo permitido es 150 caracteres.")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL es obligatoria.")]
        [MaxLength(300, ErrorMessage = "El máximo permitido es 300 caracteres.")]
        public string Path { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "El máximo permitido es 50 caracteres.")]
        #nullable enable
        public string? ContentType { get; set; }

        public long Size { get; set; }

        // Opcional: asociar imágenes a categorías existentes
        #nullable enable
        public ICollection<Guid>? CategoryIds { get; set; }
    }

}
