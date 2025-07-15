using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Movie.Data;

namespace MovieApi.Extensions
{
    public static class SeedDataExtensions
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieApiContext>();
            DbInitializer.Seed(context);
        }
    }
}
