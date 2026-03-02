using ApiImages.Models.Dtos.Image.RequestDto;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiImages.Models.Dtos.Category.RequestDto
{
    public class CategoryDto
    {
        //Retorna todos los campos del MODELO (Tabla de la db)
         public Guid Id { get; set; }

         public string Name { get; set; } = string.Empty;

         public string Description { get; set; } = string.Empty;

         public bool IsActive { get; set; }

         public DateTime CreationDate { get; set; }

        #nullable enable
        public DateTime? LastUpdate { get; set; }

        // Relación opcional: lista de imágenes asociadas
        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();


    }
}
