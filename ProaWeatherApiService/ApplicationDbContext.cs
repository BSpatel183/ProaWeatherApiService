using ProaWeatherApiService.Models;
using System.Data.Entity;

namespace ProaWeatherApiService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Server=tcp:proa-weather-challenge.database.windows.net,1433;Initial Catalog=proa.weather.challenge;Persist Security Info=False;User ID=proaBharviAdmin;Password=Proa@1379;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3000;")
        { }

        // DbSets for the entities in the database
        public virtual DbSet<WeatherStations> WeatherStations { get; set; }
        public virtual DbSet<WeatherVariables> WeatherVariables { get; set; }
        public virtual DbSet<WeatherData> WeatherData { get; set; }
    }
}
