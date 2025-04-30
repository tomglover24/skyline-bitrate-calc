namespace skyline_bitrate_calc_tom_glover.DataModels;


/// <summary>
/// Represents a JSON structure for an Arista network device containing metadata
/// about the device and its network interfaces (NICs).
/// </summary>
public class AristaResults
{
    /// <summary>
    /// Represents a JSON structure for an Arista network device, including metadata
    /// such as device name, model, and an array of network interfaces (NICs).
    /// </summary>
    public AristaResults(string device, string model, NicResults[]? nics = null)
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
    public NicResults[] Nics { get; set; }
    
    /// <summary>
    /// Device Total Received bps
    /// </summary>
    public long TotalRxBps { get; set; }
  
    /// <summary>
    /// Device Total Transmitted bps
    /// </summary>
    public long TotalTxBps { get; set; }


    /// <summary>
    /// Creates a new instance of the <see cref="AristaResults"/> class by mapping data
    /// from the provided <see cref="AristaJson"/> object. It does not map rx to tx bps.
    /// </summary>
    /// <param name="source">The source <see cref="AristaJson"/> object containing metadata
    /// of the network device and its network interfaces (NICs).</param>
    /// <returns>A new <see cref="AristaResults"/> object populated with the data
    /// from the specified <see cref="AristaJson"/> source.</returns>
    public static AristaResults CreateNew(AristaJson source)
    {
        var res = new AristaResults(source.Device, source.Model);

        res.Nics = new NicResults[source.Nics.Length];
        for (var i = 0; i < source.Nics.Length; i++)
        {
            var nic = source.Nics[i];
            res.Nics[i] = new NicResults()
            {
                Description = nic.Description,
                MAC = nic.MAC,
                Timestamp = nic.Timestamp
            };
        }

        return res;
    }
}

public class NicResults
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
    /// Received bps
    /// </summary>
    public long RxBps { get; set; }
  
    /// <summary>
    /// Transmitted bps
    /// </summary>
    public long TxBps { get; set; }
}
