using ApiImages.Models;

namespace ApiImages.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryAsync(Guid idCategory);
        Task<bool> ExistsCategoryAsync(Guid idCategory);
        Task<bool> ExistsCategoryAsync(string name);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
        Task<bool> SaveChangesAsync();
    }
}
