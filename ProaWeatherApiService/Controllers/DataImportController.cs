using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ProaWeatherApiService.Models;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class DataImportController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DataImportController(IConfiguration configuration)
    { 
        _configuration = configuration;
    }

    // POST endpoint for importing weather stations from CSV to the database
    [HttpPost("import/weatherstations")]
    public async Task<IActionResult> ImportWeatherStations([FromQuery] string filePath)
    {
        try
        {
            await ImportCsvDataAsync<WeatherStations>(filePath);
            return Ok("Weather stations data imported successfully");
        }
        catch (Exception ex)
        {
            // Catch and return any exceptions that occur during the import process
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST endpoint for importing weather variables from CSV to the database
    [HttpPost("import/variables")]
    public async Task<IActionResult> ImportVariables([FromQuery] string filePath)
    {
        try
        {
            await ImportCsvDataAsync<WeatherVariables>(filePath);
            return Ok("Variables data imported successfully");
        }
        catch (Exception ex)
        {
            // Catch and return any exceptions that occur during the import process
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST endpoint for importing weather data for a specific station from CSV to the database
    [HttpPost("import/data")]
    public async Task<IActionResult> ImportData([FromQuery] string filePath, [FromQuery] int stationId)
    {
        try
        {
            await ImportCsvDataAsync<WeatherData>(filePath, stationId);
            return Ok($"Data imported successfully for station {stationId}");
        }
        catch (Exception ex)
        {
            // Catch and return any exceptions that occur during the import process
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Generic method to import CSV data into the specified database table based on type T
    private async Task ImportCsvDataAsync<T>(string filePath, int? stationId = null)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Ignore missing field errors
            }))
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                // Open the database connection
                await connection.OpenAsync();

                // Set custom date format for DateTime fields in CSV
                csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "dd/MM/yyyy H:mm:ss" };

                // Read records from the CSV file
                var records = csv.GetRecords<T>();

                // Iterate through each record and insert into the database
                foreach (var record in records)
                {
                    // Prepare the insert command based on the type of record
                    string insertCommand = GetInsertCommand<T>();

                    // Execute the insert command within the database connection
                    using (var command = new SqlCommand(insertCommand, connection))
                    {
                        SetCommandParameters(command, record, stationId);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            // Handle file not found exception specifically
            throw new Exception($"File not found: {ex.Message}");
        }
        catch (SqlException ex)
        {
            // Handle SQL-specific exceptions
            throw new Exception($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Catch any other general exceptions
            throw new Exception($"An error occurred while importing CSV data: {ex.Message}");
        }
    }

    // Generate the appropriate SQL INSERT command based on the type T
    private string GetInsertCommand<T>()
    {
        // Determine the insert command based on the type of data being inserted
        if (typeof(T) == typeof(WeatherStations))
        {
            return "INSERT INTO WeatherStations (id, ws_name, site, portfolio, state, latitude, longitude) VALUES (@Id, @Name, @Site, @Portfolio, @State, @Latitude, @Longitude)";
        }
        else if (typeof(T) == typeof(WeatherVariables))
        {
            return "INSERT INTO WeatherVariables (var_id, id, name, unit, long_name) VALUES (@VarId, @StationId, @Name, @Unit, @LongName)";
        }
        else if (typeof(T) == typeof(WeatherData))
        {
            return "INSERT INTO WeatherData (timestamp, AirT_inst, GHI_inst, WeatherStation_Id) VALUES (@Timestamp, @AirT_inst, @GHI_inst, @WeatherStation_Id)";
        }

        // Throw an exception if an unsupported type is encountered
        throw new InvalidOperationException("Unknown type for command generation");
    }

    // Set SQL command parameters for the insert operation based on the type of record
    private void SetCommandParameters(SqlCommand command, object record, int? stationId)
    {
        // Set parameters for WeatherStations record
        if (record is WeatherStations ws)
        {
            command.Parameters.AddWithValue("@Id", ws.id);
            command.Parameters.AddWithValue("@Name", ws.ws_name);
            command.Parameters.AddWithValue("@Site", ws.site);
            command.Parameters.AddWithValue("@Portfolio", ws.portfolio);
            command.Parameters.AddWithValue("@State", ws.state);
            command.Parameters.AddWithValue("@Latitude", ws.latitude);
            command.Parameters.AddWithValue("@Longitude", ws.longitude);
        }
        // Set parameters for WeatherVariables record
        else if (record is WeatherVariables variable)
        {
            command.Parameters.AddWithValue("@VarId", variable.var_id);
            command.Parameters.AddWithValue("@StationId", variable.id);
            command.Parameters.AddWithValue("@Name", variable.name);
            command.Parameters.AddWithValue("@Unit", variable.unit);
            command.Parameters.AddWithValue("@LongName", variable.long_name);
        }
        // Set parameters for WeatherData record
        else if (record is WeatherData data)
        {
            command.Parameters.AddWithValue("@Timestamp", data.timestamp);
            command.Parameters.AddWithValue("@AirT_inst", data.AirT_inst);
            command.Parameters.AddWithValue("@GHI_inst", data.GHI_inst);
            command.Parameters.AddWithValue("@WeatherStation_Id", stationId);
        }
        else
        {
            throw new ArgumentException("Unknown record type for parameter mapping");
        }
    }
}