using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repository
{
    //Servicio que implementa la interfaz ICategoryRepository para utilizar los metodos definidos
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool UpdateCategory(Category category)
        {
            category.LastUpdate = DateTime.Now;
            _db.Category.Update(category);
            return SaveCategory();
        }

        public bool DeleteCategory(Category category)
        {
            try
            {
                category.IsActive = false;
                category.LastUpdate = DateTime.Now;
                _db.Category.Update(category);
                //_db.SaveChanges();
               // return true;
            }
            catch
            {
                return false;
            }
            //_db.Categoria.Remove(Categoria);
            return SaveCategory();

        }

        public bool CreateCategory(Category categoria)
        {
            categoria.Id = Guid.NewGuid();
            categoria.CreationDate = DateTime.Now;
            _db.Category.Add(categoria);
            return SaveCategory();
        }

        public bool ExistsCategory(Guid CategoriaId)
        {
            return _db.Category.Any(c => c.Id == CategoriaId);
        }

        public bool ExistsCategory(string nombre)
        {
            bool valor = _db.Category.Any(c => c.Name.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Category GetCategory(Guid CategoriaId)
        {
            return _db.Category.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(c => c.Name).ToList();
        }

        public bool SaveCategory()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }


    }
}

