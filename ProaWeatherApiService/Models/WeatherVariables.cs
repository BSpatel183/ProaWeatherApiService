using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProaWeatherApiService.Models
{
    [Table("WeatherVariables", Schema = "dbo")]
    public class WeatherVariables
    {
        [Key]
        public int id { get; set; }
        public int var_id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string long_name { get; set; }
    }
}
