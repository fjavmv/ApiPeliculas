
using ApiImages.Data;
using ApiImages.Mappers;
using ApiImages.Repository;
using ApiImages.Repository.IRepository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;


namespace ApiImages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
                             opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Agregamos los Repositorios
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            //Agregar el AutoMapper ya cambio en su version 15
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Configurar límites de tamaño de archivo
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024; 
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Asegurar que todos los directorios necesarios existen
            var directoriesToCreate = new[]
            {
    Path.Combine(app.Environment.ContentRootPath, "uploads"),
    Path.Combine(app.Environment.ContentRootPath, "wwwroot"),
    Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads")
};

            foreach (var directory in directoriesToCreate)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    app.Logger.LogInformation("Directorio creado: {Directory}", directory);
                }
            }

            app.UseStaticFiles(); // sirve wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "uploads")),
                RequestPath = "/uploads"
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
