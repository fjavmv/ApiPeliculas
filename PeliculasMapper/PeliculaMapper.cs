using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculaMapper: Profile
    {
        public PeliculaMapper()
        {
            CreateMap<Categoria, CategoryDto>().ReverseMap();
            CreateMap<Categoria, CreateCategoryDto>().ReverseMap();
        }
    }
}
