using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryDto
    {
        //Retorna todos los campos del MODELO (Tabla de la db)
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime FechaDeCreacion { get; set; }
    }
}
