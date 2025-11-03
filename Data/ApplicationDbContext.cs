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
        public DbSet<Category> Category { get; set; }

        /* 
        - automáticamente se aplicará el filtro WHERE IsActive = 1.
        - No necesitas poner Where(c => c.IsActive) en cada consulta.
        - Si quieres incluir también los inactivos en un caso puntual, puedes usar IgnoreQueryFilters():
         */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtro global
            modelBuilder.Entity<Category>().HasQueryFilter(c => c.IsActive);
        }

    }
}
