using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")] //Opcion estatica
    [ApiController] //Opcion dinamica
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoryRepository _ctRepo, IMapper  mapper)
        {
            _catRepo = _ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCategorys()
        {
            var listaCategorias = _catRepo.GetCategorys();
            var listaCategoriasDto = new List<CategoryDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoryDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }
        
    }
}
