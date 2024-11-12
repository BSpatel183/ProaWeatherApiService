using System.ComponentModel.DataAnnotations.Schema;

namespace ProaWeatherApiService.Models
{
    [Table("WeatherStations", Schema = "dbo")]
    public class WeatherStations
    {
        public int id { get; set; }
        public string ws_name { get; set; }
        public string site { get; set; }
        public string portfolio { get; set; }
        public string state { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
    }
}
