using ApiImages.Models;

namespace ApiImages.Repository.IRepository
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetImagesAsync();
        Task<Image?> GetImageByIdAsync(Guid idImage);
        Task<bool> ExistsImageAsync(Guid idImage);
        Task<bool> ExistsImageAsync(string fileName);
        Task AddImageAsync(Image image);
        Task UpdateImageAsync(Image image);
        Task DeleteImageAsync(Image image);
        Task<bool> SaveChangesAsync();
        // Métodos adicionales
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Image>> SearchImagesAsync(string? query, List<Guid>? categoryIds, int page, int pageSize);
        Task<IEnumerable<Image>> GetImagesByCategoryAsync(Guid categoryId);
        Task<List<Category>> GetCategoriesByIdsAsync(List<Guid> categoryIds);
        Task AddCategoriesToImageAsync(Guid imageId, List<Guid> categoryIds);
    }
}
