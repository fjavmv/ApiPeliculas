using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")] //Opcion estatica
    [ApiController] //Opcion dinamica
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IMapper _mapper;

        //Inyeccion de dependencias
        public CategorysController(ICategoryRepository _ctRepo, IMapper  mapper)
        {
            _catRepo = _ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategorys()
        {
            var listCategorys = _catRepo.GetCategorys();
            var listCategorysDto = new List<CategoryDto>();

            foreach (var list in listCategorys)
            {
                listCategorysDto.Add(_mapper.Map<CategoryDto>(list));
            }
            return Ok(listCategorysDto);
        }
        
    }
}
