using System.ComponentModel.DataAnnotations.Schema;

namespace ProaWeatherApiService.Models
{
    [Table("WeatherData", Schema = "dbo")]
    public class WeatherData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public decimal AirT_inst { get; set; }
        public decimal GHI_inst { get; set; }
        public DateTime timestamp { get; set; }
        public int WeatherStation_Id { get; set; }
        [ForeignKey("WeatherStation_Id")]
        public virtual WeatherStations WeatherStations { get; set; }

    }
}