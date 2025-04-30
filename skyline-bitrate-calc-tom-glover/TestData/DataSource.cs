using System.Text.Json;
using System.Text.Json.Serialization;
using skyline_bitrate_calc_tom_glover.DataModels;

namespace skyline_bitrate_calc_tom_glover;

public static class DataSource
{
    /// <summary>
    /// <para>Asynchronously retrieves serialised network device data in JSON format. The retrieved data
    /// includes metadata such as the device name, model, and an array of network interface objects (NICs).</para>
    /// <para>This currently generates placeholder data with current time and random RX/TX values per nic.</para>
    /// </summary>
    /// <returns>
    /// A task result contains a JSON string representing the network device data.
    /// </returns>
    public static Task<string> GetDataAsync()
    {
        var nics = new Nic[]
        {
            new Nic
            {
                Description = "Linksys ABR",
                MAC = "14:91:82:3C:D6:7D",
                Timestamp = DateTime.UtcNow,
                Rx = Random.Shared.NextInt64(1_000_000, 1_000_000_000),
                Tx = Random.Shared.NextInt64(1_000, 1_000_000)
            }
        };
        
        var data = new AristaJson("Arista", "X-Video", nics);

        return Task.FromResult(JsonSerializer.Serialize(data, new JsonSerializerOptions(){NumberHandling = JsonNumberHandling.WriteAsString}));
    }
    
    /// <summary>
    /// <para>Asynchronously retrieves serialised network device data in JSON format. The retrieved data
    /// includes metadata such as the device name, model, and an array of network interface objects (NICs).</para>
    /// <para>This currently generates placeholder data with current time and random RX/TX values per nic.</para>
    /// </summary>
    /// <returns>
    /// A  task result contains a JSON string representing the network device data.
    /// </returns>
    public static Task<string> GetMultiNicDataAsync()
    {

        var reportDateTime = DateTime.UtcNow;
        var nics = new Nic[]
        {
            new Nic
            {
                Description = "Linksys ABR",
                MAC = "14:91:82:3C:D6:7D",
                Timestamp = reportDateTime,
                Rx = Random.Shared.NextInt64(1_000_000, 1_000_000_000),
                Tx = Random.Shared.NextInt64(1_000, 1_000_000)
            },
            new Nic
            {
            Description = "Linksys ABR 2",
            MAC = "14:91:82:3C:D6:7E",
            Timestamp = reportDateTime,
            Rx = Random.Shared.NextInt64(1_000_000, 1_000_000_000),
            Tx = Random.Shared.NextInt64(1_000, 1_000_000)
            }
        };
        
        var data = new AristaJson("Arista", "X-Video", nics);

        return Task.FromResult(JsonSerializer.Serialize(data, new JsonSerializerOptions(){NumberHandling = JsonNumberHandling.WriteAsString}));
    }
}