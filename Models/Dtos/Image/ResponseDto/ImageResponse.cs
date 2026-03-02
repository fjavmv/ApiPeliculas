using ApiImages.Models.Dtos.Category.RequestDto;

namespace ApiImages.Models.Dtos.Image.ResponseDto
{
    /// <summary>
    /// DTO para exponer información completa de una imagen almacenada.
    /// </summary>
    public class ImageResponse
    {
        /// <summary>
        /// Identificador único de la imagen.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del archivo de la imagen.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// URL pública o interna donde se encuentra la imagen.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Tipo MIME del contenido (por ejemplo, image/jpeg).
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Tamaño del archivo en bytes.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Fecha de creación de la imagen.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Indica si la imagen está activa o disponible.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Categorías asociadas a esta imagen (si se desea incluir).
        /// </summary>
        public ICollection<CategoryDto> Categories { get; set; }
    }
}
