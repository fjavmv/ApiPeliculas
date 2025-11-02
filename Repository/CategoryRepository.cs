using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

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

        public bool UpdateCategory(Categoria categoria)
        {
            categoria.CretioDate = DateTime.Now;
            _db.Categoria.Update(categoria);
            return SaveCategory();
        }

        public bool DeleteCategory(Categoria Categoria)
        {
            _db.Categoria.Remove(Categoria);
            return SaveCategory();

        }

        public bool CreateCategory(Categoria categoria)
        {
            categoria.Id = Guid.NewGuid();
            categoria.CretioDate = DateTime.Now;
            _db.Categoria.Add(categoria);
            return SaveCategory();
        }

        public bool ExistsCategory(Guid CategoriaId)
        {
            return _db.Categoria.Any(c => c.Id == CategoriaId);
        }

        public bool ExistsCategory(string nombre)
        {
            bool valor = _db.Categoria.Any(c => c.Name.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Categoria GetCategory(Guid CategoriaId)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorys()
        {
            return _db.Categoria.OrderBy(c => c.Name).ToList();
        }

        public bool SaveCategory()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }


    }
}

