using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Route("api/[controller]")] //Opcion estatica
    [Route("api/categories")]
    [ApiController] //Opcion dinamica
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IMapper _mapper;

        //Inyeccion de dependencias
        public CategoriesController(ICategoryRepository _ctRepo, IMapper  mapper)
        {
            _catRepo = _ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategories()
        {
            var listCategories = _catRepo.GetCategories();
            var listCategoryDto = new List<CategoryDto>();

            foreach (var list in listCategories)
            {
                listCategoryDto.Add(_mapper.Map<CategoryDto>(list));
            }
            return Ok(listCategoryDto);
        }

     
        [HttpGet("{idCategory:Guid}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategory(Guid idCategory)
        {

            CategoryDto itemCategoryDto = null;

            try
            {
                var categoryItem = _catRepo.GetCategory(idCategory);
                if (categoryItem == null)
                {
                    return NotFound("No se obtuvieron resultados de tu busqueda.");
                }
                itemCategoryDto = _mapper.Map<CategoryDto>(categoryItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor.");
            }

            return Ok(itemCategoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoryCreateDto == null) return BadRequest("No puede ser vacío.");

            if (_catRepo.ExistsCategory(categoryCreateDto.Name)) 
            {
              
                ModelState.AddModelError("", $"La categoria ya existe.");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(categoryCreateDto);

            if (!_catRepo.CreateCategory(category)) 
            {
                ModelState.AddModelError("", $"Algo ha salido mal al guardar registro {category.Name}.");
                return StatusCode(500,ModelState); 
            }

            return CreatedAtRoute("GetCategory", new {idCategory = category.Id}, category);

        }

        [HttpPatch("{idCategory:guid}", Name ="UpdatePatchCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchCategory(Guid idCategory, [FromBody] CategoryPatchDto categoryPatchDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoryPatchDto == null || idCategory != categoryPatchDto.Id) return BadRequest(ModelState);


            if (!_catRepo.ExistsCategory(categoryPatchDto.Id))
            {

                return NotFound("La categoría no existe.");
            }

            var category = _mapper.Map<Category>(categoryPatchDto);

            if (!_catRepo.UpdateCategory(category))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al actualizar la categoría",
                    Detail = $"Algo ha salido mal al actualizar el registro {category.Name}.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();

        }

        [HttpPut("{idCategory:guid}", Name = "UpdatePutCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePutCategory(Guid idCategory, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoryUpdateDto == null || idCategory != categoryUpdateDto.Id) return BadRequest(ModelState);


            if (!_catRepo.ExistsCategory(categoryUpdateDto.Id))
            {

                return NotFound("La categoría no existe.");
            }

            var category = _mapper.Map<Category>(categoryUpdateDto);

            if (!_catRepo.UpdateCategory(category))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al actualizar la categoría",
                    Detail = $"Algo ha salido mal al actualizar el registro {category.Name}.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();

        }


        [HttpDelete("{idCategory:guid}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(Guid idCategory)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_catRepo.ExistsCategory(idCategory))
            {

                return NotFound("El ID buscado no existe o ya ha sido eliminado.");
            }

            var category = _catRepo.GetCategory(idCategory);

            if (!_catRepo.DeleteCategory(category))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al borrar la categoría",
                    Detail = $"Algo ha salido mal al actualizar el registro {category.Name}.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();

        }


    }
}
