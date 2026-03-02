using ApiImages.Data;
using ApiImages.Models;
using ApiImages.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiImages.Repository
{
    //Servicio que implementa la interfaz ICategoryRepository para utilizar los metodos definidos
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Category
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(Guid idCategory)
        {
            return await _db.Category
                .FirstOrDefaultAsync(c => c.Id == idCategory);
        }

        public async Task<bool> ExistsCategoryAsync(Guid idCategory)
        {
            return await _db.Category.AnyAsync(c => c.Id == idCategory);
        }

        public async Task<bool> ExistsCategoryAsync(string name)
        {
            return await _db.Category
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task AddCategoryAsync(Category category)
        {
            category.Id = Guid.NewGuid();
            category.CreationDate = DateTime.UtcNow;

            await _db.Category.AddAsync(category);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            category.LastUpdate = DateTime.UtcNow;
            _db.Category.Update(category);
            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(Category category)
        {
            category.IsActive = false;
            category.LastUpdate = DateTime.UtcNow;
            _db.Category.Update(category);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }

}

