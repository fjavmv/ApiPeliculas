using ApiImages.Models.Dtos.Category.RequestDto;

namespace ApiImages.Models.Dtos.Image.RequestDto
{
    public class ImageDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Size { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? LastUpdate { get; set; }

        public bool IsActive { get; set; }

        // Relación muchos a muchos → categorías asociadas
        #nullable enable
        public ICollection<CategoryDto>? Categories { get; set; }
    }
}
