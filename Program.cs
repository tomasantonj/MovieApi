using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models; // Add for Swagger
using Movie.Contracts;
using Movie.Data;
using Movie.Services;
using MovieApi.Extensions;
using MovieApi.Movie.Core.DomainContracts;
using MovieApi.Movie.Data.Repositories;
using MovieApi.Middleware;

namespace MovieApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MovieApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieApiContext") ?? throw new InvalidOperationException("Connection string 'MovieApiContext' not found.")));

            // Register repositories
            builder.Services.AddScoped<IVideoMovieRepository, VideoMovieRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            // Register UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Register MovieService
            builder.Services.AddScoped<IMovieService, MovieService>();
            // Register ActorService
            builder.Services.AddScoped<IActorService, ActorService>();
            // Register BusinessRulesService
            builder.Services.AddScoped<IBusinessRulesService, BusinessRulesService>();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(); // Register Swagger

            var app = builder.Build();

            // Add error handling middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Seed the database
                app.Seed();
                // Enable Swagger middleware
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API V1");
                    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
