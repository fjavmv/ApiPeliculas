using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryDto
    {
        //Retorna todos los campos del MODELO (Tabla de la db)
         public Guid Id { get; set; }

         public string Name { get; set; } = string.Empty;

         public string Description { get; set; } = string.Empty;

         public bool IsActive { get; set; }

         public DateTime CreationDate { get; set; }

         public DateTime LastUpdate { get; set; }

    }
}
