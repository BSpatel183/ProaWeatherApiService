using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProaWeatherApiService.Models
{
    [Table("WeatherVariables", Schema = "dbo")]
    public class WeatherVariables
    {
        [Key]
        public int var_id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string long_name { get; set; }
        public int id { get; set; }
        public int WeatherStation_Id { get; set; }
        [JsonIgnore]
        [ForeignKey("WeatherStation_Id")]
        public virtual WeatherStations WeatherStations { get; set; }
    }
}
