using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.Mappers
{
   public class PeliculasMapper: Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoryDto>().ReverseMap();
            CreateMap<Categoria, CreateCategoryDto>().ReverseMap();
            CreateMap<Categoria, UpdateCategoryDto>().ReverseMap();
        }
    }
}
