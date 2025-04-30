using System.Text.Json.Serialization;

namespace skyline_bitrate_calc_tom_glover.DataModels;


/// <summary>
/// Represents a JSON structure for an Arista network device containing metadata
/// about the device and its network interfaces (NICs).
/// </summary>
public class AristaJson
{
    /// <summary>
    /// Represents a JSON structure for an Arista network device containing metadata
    /// such as the device name, model, and associated network interfaces (NICs).
    /// </summary>
    public AristaJson()
    {
        Device = "Arista";
        Model = "X-Video";
        Nics = [];
    }

    /// <summary>
    /// Represents a JSON structure for an Arista network device, including metadata
    /// such as device name, model, and an array of network interfaces (NICs).
    /// </summary>
    public AristaJson(string device, string model, Nic[]? nics = null)
    {
        Device = device;
        Model = model;
        if (nics is not null)
        {
            Nics = nics;
        }
        else
        {
            Nics = [];
        }
    }
    
    /// <summary>
    /// Represents the Device Name
    /// </summary>
    public string Device { get; set; }
    /// <summary>
    /// Represents the Device Model
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// Represents the data provided about one or more network interfaces
    /// </summary>
    [JsonPropertyName("NIC")]
    public Nic[] Nics { get; set; }
}

public class Nic
{
    /// <summary>
    /// Description of the Network Interface
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// MAC Address of the Network Interface
    /// </summary>
    public string MAC { get; set; }
    /// <summary>
    /// Timestamp of the Mesurment
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Received Octets (bytes / 8 bits) in the last reporting period
    /// </summary>
    public long Rx { get; set; }
  
    /// <summary>
    /// Transmitted Octets (bytes / 8 bits) in the last reporting period
    /// </summary>
    public long Tx { get; set; }
}