using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProaWeatherApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherStationsController : ControllerBase
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        // Constructor to inject the ApplicationDbContext
        public WeatherStationsController(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllWeatherStations()
        {
            var context = new ApplicationDbContext(_options);
            var weatherStations = await context.WeatherStations
                .ToListAsync();
            return Ok(weatherStations);
        }


        // GET: api/weatherstations/{id}/latestdata
        [HttpGet, Route("{id:int}/latestdata")]
        public async Task<IActionResult> GetLatestDataByWeatherStation(int id)
        {
            var context = new ApplicationDbContext(_options);
            var weatherData = await context.WeatherData
                .Include(x => x.WeatherStations)
                    .ThenInclude(x => x.WeatherVariables)
                .Where(x => x.WeatherStation_Id == id)
                .OrderByDescending(d => d.timestamp)
                .FirstOrDefaultAsync();

            if (weatherData == null)
            {
                return NotFound();
            }

            return Ok(weatherData);
        }
    }
}