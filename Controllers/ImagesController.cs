using ApiImages.Data;
using ApiImages.Models;
using ApiImages.Models.Dtos.Category;
using ApiImages.Models.Dtos.Image.RequestDto;
using ApiImages.Models.Dtos.Image.ResponseDto;
using ApiImages.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ApiImages.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imgRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImagesController> _logger;
        private readonly ApplicationDbContext _db; // Agregar DbContext para transacciones

        public ImagesController(IImageRepository imgRepo,IMapper mapper,IWebHostEnvironment environment,ILogger<ImagesController> logger,ApplicationDbContext db) // Inyectar DbContext
        {
            _imgRepo = imgRepo;
            _mapper = mapper;
            _environment = environment;
            _logger = logger;
            _db = db;
        }

        // Mantener los otros métodos existentes...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetImages()
        {
            var listImages = await _imgRepo.GetImagesAsync();
            var listImageDto = _mapper.Map<IEnumerable<ImageDto>>(listImages);
            return Ok(listImageDto);
        }

        [HttpGet("{idImage:Guid}", Name = "GetImage")]
        public async Task<ActionResult<ImageDto>> GetImage(Guid idImage)
        {
            var image = await _imgRepo.GetImageByIdAsync(idImage);
            if (image == null)
                return NotFound(new { Message = "Imagen no encontrada" });

            return Ok(_mapper.Map<ImageDto>(image));
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ImageResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<ImageResponse>> UploadImageWithFile(
      [FromForm] ImageUploadRequest uploadRequest)
        {
            try
            {
                //  Validaciones básicas
                if (uploadRequest?.File == null || uploadRequest.File.Length == 0)
                    return BadRequest(new { Message = "Archivo no válido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!IsValidImageFile(uploadRequest.File))
                    return BadRequest(new { Message = "Tipo de archivo no permitido" });

                if (uploadRequest.File.Length > 100 * 1024 * 1024)
                    return BadRequest(new { Message = "El archivo es demasiado grande" });

                //  Crear directorio de uploads si no existe
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                //  Generar nombres únicos y seguros
                var fileExtension = Path.GetExtension(uploadRequest.File.FileName).ToLowerInvariant();
                var uniqueId = Guid.NewGuid();
                var originalFileName = Path.GetFileNameWithoutExtension(uploadRequest.File.FileName);
                var safeFileName = SanitizeFileName(originalFileName);
                var fileName = $"{safeFileName}_{uniqueId}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                //  Guardar archivo físico
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadRequest.File.CopyToAsync(stream);
                }

                var friendlyName = string.IsNullOrWhiteSpace(uploadRequest.FileName)
                    ? originalFileName
                    : uploadRequest.FileName;

                //  Crear entidad imagen
                var image = new Image
                {
                    Id = uniqueId,
                    FileName = friendlyName,
                    Path = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}",
                    ContentType = uploadRequest.File.ContentType,
                    Size = uploadRequest.File.Length,
                    CreationDate = DateTime.UtcNow,
                    IsActive = true
                };

                //  Guardar imagen primero en BD
                await _imgRepo.AddImageAsync(image);

                //  Asociar categorías si se enviaron
                if (uploadRequest.CategoryIds != null && uploadRequest.CategoryIds.Any())
                {
                    await _imgRepo.AddCategoriesToImageAsync(image.Id, uploadRequest.CategoryIds);
                }

                //  Recargar la imagen con categorías incluidas
                var completeImage = await _imgRepo.GetImageByIdAsync(image.Id);

                _logger.LogInformation("Imagen subida exitosamente: {FileName} (ID: {ImageId})", fileName, image.Id);

                //  Mapear a DTO y devolver en body
                var resultDto = _mapper.Map<ImageResponse>(completeImage);
                return CreatedAtRoute("GetImage", new { idImage = image.Id }, resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir imagen");
                return StatusCode(500, new
                {
                    Message = "Error interno del servidor",
                    Detailed = ex.Message
                });
            }
        }

        [HttpDelete("{idImage:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImage(Guid idImage)
        {
            try
            {
                var image = await _imgRepo.GetImageByIdAsync(idImage);
                if (image == null)
                {
                    _logger.LogWarning("Intento de eliminar imagen no encontrada: {ImageId}", idImage);
                    return NotFound(new { Message = "Imagen no encontrada" });
                }

                // Eliminar archivo físico si existe
                try
                {
                    if (image.Path.StartsWith("/uploads/"))
                    {
                        // ✅ SOLUCIÓN: Usar nombres diferentes
                        var physicalFileName = Path.GetFileName(image.Path);
                        var physicalFilePath = Path.Combine(_environment.ContentRootPath, "uploads", physicalFileName);

                        // ✅ SOLUCIÓN: Usar System.IO.File explícitamente
                        if (System.IO.File.Exists(physicalFilePath))
                        {
                            System.IO.File.Delete(physicalFilePath);
                            _logger.LogInformation("Archivo físico eliminado: {FilePath}", physicalFilePath);
                        }
                    }
                }
                catch (Exception fileEx)
                {
                    _logger.LogWarning(fileEx, "No se pudo eliminar el archivo físico de la imagen {ImageId}", idImage);
                    // Continuar con la eliminación de la BD
                }

                await _imgRepo.DeleteImageAsync(image);

                _logger.LogInformation("Imagen eliminada exitosamente: {FileName} (ID: {ImageId})",
                    image.FileName, image.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen {ImageId}", idImage);
                return StatusCode(500, new
                {
                    Message = "Error al eliminar la imagen",
                    Detailed = ex.Message
                });
            }
        }


        // Métodos helper
        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(fileName
                .Where(ch => !invalidChars.Contains(ch))
                .ToArray());

            return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        }

        private bool IsValidImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var allowedContentTypes = new[]
            {
            "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp"
        };

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension) &&
                   allowedContentTypes.Contains(file.ContentType);
        }

    }
}
