using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiImages.Models.Dtos.Category.ResponseDto
{
    public class DeleteCategoryResponse
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }   // opcional

        public string Description { get; set; }   // opcional
    }
}
