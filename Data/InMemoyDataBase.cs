using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FilmStudioSFF.Data;

namespace FilmStudioSFF
{
    public static class InMemoryDatabase
    {
        public static void AddInMemoryDatabase(this IServiceCollection services)
        {
            services.AddDbContext<FilmStudioDbContext>(options =>
                options.UseInMemoryDatabase("FilmStudioDb"));
        }
    }
}
