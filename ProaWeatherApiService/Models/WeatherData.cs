using System.ComponentModel.DataAnnotations.Schema;

namespace ProaWeatherApiService.Models
{
    [Table("WeatherData", Schema = "dbo")]
    public class WeatherData
    {
        public int id { get; set; }
        public decimal AirT_inst { get; set; }
        public decimal GHI_inst { get; set; }
        public DateTime timestamp { get; set; }
    }
}
