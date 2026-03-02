using ApiImages.Models;
using ApiImages.Models.Dtos.Category.RequestDto;
using ApiImages.Models.Dtos.Category.ResponseDto;
using ApiImages.Repository.IRepository;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace ApiImages.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var listCategories = await _catRepo.GetCategoriesAsync();
            var listCategoryDto = _mapper.Map<IEnumerable<CategoryDto>>(listCategories);
            return Ok(listCategoryDto);
        }


        [HttpGet("{idCategory:Guid}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategory(Guid idCategory)
        {
            try
            {
                var categoryItem = await _catRepo.GetCategoryAsync(idCategory);
                if (categoryItem == null)
                    return NotFound("No se obtuvieron resultados de tu búsqueda.");

                var itemCategoryDto = _mapper.Map<CategoryDto>(categoryItem);
                return Ok(itemCategoryDto);
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest categoryCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (categoryCreateDto == null) return BadRequest("No puede ser vacío.");

            if (await _catRepo.ExistsCategoryAsync(categoryCreateDto.Name))
            {
                ModelState.AddModelError("", "La categoría ya existe.");
                return StatusCode(409, ModelState); // 409 Conflict es más apropiado que 404
            }

            var category = _mapper.Map<Category>(categoryCreateDto);
            await _catRepo.AddCategoryAsync(category);

            if (!await _catRepo.SaveChangesAsync())
            {
                ModelState.AddModelError("", $"Algo ha salido mal al guardar el registro {category.Name}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { idCategory = category.Id }, category);
        }

        [HttpPatch("{idCategory:guid}", Name = "UpdatePatchCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatchCategory(Guid idCategory, [FromBody] DeleteCategoryRequest categoryPatchDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (categoryPatchDto == null || idCategory != categoryPatchDto.Id) return BadRequest(ModelState);

            if (!await _catRepo.ExistsCategoryAsync(idCategory))
                return NotFound("La categoría no existe.");

            var category = _mapper.Map<Category>(categoryPatchDto);
            await _catRepo.UpdateCategoryAsync(category);

            if (!await _catRepo.SaveChangesAsync())
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
        public async Task<IActionResult> UpdatePutCategory(Guid idCategory, [FromBody] CategoryDetailResponse categoryUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (categoryUpdateDto == null || idCategory != categoryUpdateDto.Id) return BadRequest(ModelState);

            if (!await _catRepo.ExistsCategoryAsync(idCategory))
                return NotFound("La categoría no existe.");

            var category = _mapper.Map<Category>(categoryUpdateDto);
            await _catRepo.UpdateCategoryAsync(category);

            if (!await _catRepo.SaveChangesAsync())
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
        public async Task<IActionResult> DeleteCategory(Guid idCategory)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _catRepo.ExistsCategoryAsync(idCategory))
                return NotFound("El ID buscado no existe o ya ha sido eliminado.");

            var category = await _catRepo.GetCategoryAsync(idCategory);
            if (category == null)
                return NotFound("La categoría no fue encontrada.");

            await _catRepo.DeleteCategoryAsync(category);

            if (!await _catRepo.SaveChangesAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al borrar la categoría",
                    Detail = $"Algo ha salido mal al eliminar el registro {category.Name}.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }


    }
}
