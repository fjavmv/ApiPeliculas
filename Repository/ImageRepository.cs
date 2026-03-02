using ApiImages.Data;
using ApiImages.Models;
using ApiImages.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiImages.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _db;

        public ImageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddImageAsync(Image image)
        {
            image.Id = Guid.NewGuid();
            image.CreationDate = DateTime.UtcNow;
            image.IsActive = true;

            if (image.Categories?.Any() == true)
            {
                var categories = image.Categories.ToList();
                image.Categories.Clear();

                await _db.Image.AddAsync(image);
                await _db.SaveChangesAsync();

                foreach (var category in categories)
                {
                    var existingCategory = await _db.Category.FindAsync(category.Id);
                    if (existingCategory != null &&
                        !image.Categories.Any(c => c.Id == existingCategory.Id))
                    {
                        image.Categories.Add(existingCategory);
                    }
                }

                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.Image.AddAsync(image);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteImageAsync(Image image)
        {
            // SOLUCIÓN: Eliminar relaciones primero
            image.Categories.Clear(); // Limpiar relaciones muchos a muchos
            await _db.SaveChangesAsync();

            _db.Image.Remove(image);
            await SaveChangesAsync();
        }

        public async Task<bool> ExistsImageAsync(Guid idImage)
        {
            return await _db.Image.AnyAsync(i => i.Id == idImage);
        }

        public async Task<bool> ExistsImageAsync(string fileName)
        {
            return await _db.Image.AnyAsync(i => i.FileName == fileName);
        }

        public async Task<Image?> GetImageByIdAsync(Guid idImage)
        {
            return await _db.Image
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == idImage);
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            return await _db.Image
                .Include(i => i.Categories)
                .Where(i => i.IsActive)
                .OrderBy(i => i.FileName)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine($"INNER EXCEPTION: {innerException.Message}");
                    innerException = innerException.InnerException;
                }
                throw;
            }
        }

        public async Task UpdateImageAsync(Image image)
        {
            _db.Image.Update(image);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Category
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Image>> SearchImagesAsync(string? query, List<Guid>? categoryIds, int page = 1, int pageSize = 20)
        {
            var imagesQuery = _db.Image
                .Include(i => i.Categories)
                .Where(i => i.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                imagesQuery = imagesQuery.Where(i =>
                    i.FileName.Contains(query) ||
                    i.Path.Contains(query) ||
                    i.Categories.Any(c => c.Name.Contains(query)));
            }

            if (categoryIds != null && categoryIds.Any())
            {
                imagesQuery = imagesQuery.Where(i =>
                    i.Categories.Any(c => categoryIds.Contains(c.Id)));
            }

            return await imagesQuery
                .OrderByDescending(i => i.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Image>> GetImagesByCategoryAsync(Guid categoryId)
        {
            return await _db.Image
                .Include(i => i.Categories)
                .Where(i => i.IsActive && i.Categories.Any(c => c.Id == categoryId))
                .OrderBy(i => i.FileName)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesByIdsAsync(List<Guid> categoryIds)
        {
            return await _db.Category
                .Where(c => categoryIds.Contains(c.Id) && c.IsActive)
                .ToListAsync();
        }

        // NUEVO MÉTODO: Para manejar la relación muchos a muchos correctamente
        public async Task AddCategoriesToImageAsync(Guid imageId, List<Guid> categoryIds)
        {
            var image = await _db.Image
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null) return;

            var categories = await _db.Category
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var category in categories)
            {
                if (!image.Categories.Any(c => c.Id == category.Id))
                {
                    image.Categories.Add(category);
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
