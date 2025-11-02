using ApiPeliculas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Aqui se colocan todos los modelos (entidades) para tener acceso
        public DbSet<Categoria> Categoria { get; set; }
    }
}
