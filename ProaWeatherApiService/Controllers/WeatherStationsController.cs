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
            return Ok();
        }
    }
}