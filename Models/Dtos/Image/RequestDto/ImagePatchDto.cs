using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Image.RequestDto
{
    public class ImagePatchDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(150)]
        #nullable enable
        public string? FileName { get; set; }

        [MaxLength(300)]
        #nullable enable
        public string? Path { get; set; }

        [MaxLength(50)]
        #nullable enable
        public string? ContentType { get; set; }
        
        #nullable enable
        public long? Size { get; set; }

        #nullable enable
        public ICollection<Guid>? CategoryIds { get; set; }
    }
}
