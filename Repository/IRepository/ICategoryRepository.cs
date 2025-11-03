using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(Guid CategoriaId);
        bool ExistsCategory(Guid CategoriaId);
        bool ExistsCategory(string nombre);
        bool CreateCategory(Category categoria);
        bool UpdateCategory(Category categoria);
        bool DeleteCategory(Category categoria);
        bool SaveCategory();

    }
}
