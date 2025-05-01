using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class CategoyRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoyRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateCategory(Categoria categoria)
        {
            _db.Categoria.Add(categoria);
            return SaveCategory();
        }

        public bool DeleteCategory(Categoria Categoria)
        {
            _db.Categoria.Remove(Categoria);
            return SaveCategory();

        }

        public bool ExistsCategory(int CategoriaId)
        {
            return _db.Categoria.Any(c => c.Id == CategoriaId);
        }

        public bool ExistsCategory(string nombre)
        {
            bool valor = _db.Categoria.Any(c => c.Name.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Categoria GetCategory(int CategoriaId)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorys()
        {
            return _db.Categoria.OrderBy(c => c.Name).ToList();
        }

        public bool SaveCategory()
        {
            return _db.SaveChanges() >= 0 ? true: false;
        }

        public bool UpdateCategory(Categoria categoria)
        {
            categoria.FechaDeCreacion = DateTime.Now;
            _db.Categoria.Update(categoria);
            return SaveCategory();
        }
    }
}
