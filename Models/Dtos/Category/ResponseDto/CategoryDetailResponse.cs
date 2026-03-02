using ApiImages.Models.Dtos.Category.RequestDto;
using System.ComponentModel.DataAnnotations;

namespace ApiImages.Models.Dtos.Category.ResponseDto
{
    public class CategoryDetailResponse : CreateCategoryRequest
    {
        [Required]
        public Guid Id { get; set; }

    }
}
