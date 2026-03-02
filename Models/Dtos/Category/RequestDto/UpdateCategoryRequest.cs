using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Category.RequestDto
{
    public class UpdateCategoryResponse : CreateCategoryRequest
    {
        [Required]
        public Guid Id { get; set; }

    }
}
