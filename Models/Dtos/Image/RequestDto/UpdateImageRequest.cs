using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Image.RequestDto
{
    public class UpdateImageRequest : CreateImageRequest
    {
        [Required]
        public Guid Id { get; set; }

    }
}
