using Microsoft.EntityFrameworkCore;
using ProaWeatherApiService.Models;

namespace ProaWeatherApiService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for the entities in the database
        public virtual DbSet<WeatherStations> WeatherStations { get; set; }
        public virtual DbSet<WeatherVariables> WeatherVariables { get; set; }
        public virtual DbSet<WeatherData> WeatherData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherData>()
                .HasKey(wd => wd.id);
        }
    }
}
