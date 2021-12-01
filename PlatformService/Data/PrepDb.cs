using PlatformService.Models;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),env);
            }
        }

        private static void SeedData(AppDbContext context, IWebHostEnvironment env)
        {
            if(env.IsProduction())
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try 
                {
                   context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations {ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");  
                context.Platforms.AddRange(
                    new Platform() {Name="Dot Net",Publisher="Microsoft",Cost="Free"},
                    new Platform() {Name="Sql",Publisher="Microsoft",Cost="Free"},
                    new Platform() {Name="Kubernetes",Publisher="Cloud Native",Cost="Free"}


                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data!");
            }
        }
    }
}