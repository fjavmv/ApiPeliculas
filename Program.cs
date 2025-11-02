
using ApiPeliculas.Data;
using ApiPeliculas.Mappers;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace ApiPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
                             opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Agregamos los Repositorios
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            //Agregar el AutoMapper ya cambio en su version 15
            builder.Services.AddAutoMapper(typeof(PeliculasMapper));



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
