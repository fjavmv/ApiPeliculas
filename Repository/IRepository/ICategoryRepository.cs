using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Categoria> GetCategorys();
        Categoria GetCategory(Guid CategoriaId);
        bool ExistsCategory(Guid CategoriaId);
        bool ExistsCategory(string nombre);
        bool CreateCategory(Categoria categoria);
        bool UpdateCategory(Categoria categoria);
        bool DeleteCategory(Categoria categoria);
        bool SaveCategory();

    }
}
