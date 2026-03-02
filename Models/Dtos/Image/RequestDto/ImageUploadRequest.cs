using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Image.RequestDto
{
    public class ImageUploadRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El archivo es obligatorio")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es obligatorio")]
        [MaxLength(150, ErrorMessage = "El máximo permitido es 150 caracteres")]
        public string FileName { get; set; } = string.Empty;

        public List<Guid> CategoryIds { get; set; }
    }
}
