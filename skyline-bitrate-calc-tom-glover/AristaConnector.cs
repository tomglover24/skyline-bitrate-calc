using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.Utilities;
using skyline_bitrate_calc_tom_glover.DataModels;

namespace skyline_bitrate_calc_tom_glover;

/// <summary>
/// Represents a connector for interacting with an Arista network device.
/// The connector provides mechanisms for periodic polling of network statistics
/// and retrieving calculated results from the collected data.
/// </summary>
public class AristaConnector
{
    /// <summary>
    /// Polling Frequency in Hz
    /// </summary>
    private readonly double _pollingFrequency = 2.0;

    /// <summary>
    /// Polling interval in time, derived from polling frequency...
    /// </summary>
    private readonly TimeSpan _pollingInterval;

    /// <summary>
    /// Contains default options for JSON serialisation, enabling the handling of numbers
    /// read as strings during deserialization.
    /// </summary>
    private readonly JsonSerializerOptions _defaultSerializerOptions = new JsonSerializerOptions() {NumberHandling = JsonNumberHandling.AllowReadingFromString};


    /// <summary>
    /// Circular buffer storing recent polling data retrieved from the Arista network device.
    /// Used to calculate aggregated results and track historical network statistics.
    /// </summary>
    private readonly CircularBuffer<AristaJson> _pollingBuffer;
    
    public AristaConnector()
    {
        _pollingInterval = TimeSpan.FromSeconds(1 / _pollingFrequency);
        _pollingBuffer = new CircularBuffer<AristaJson>((int)Math.Ceiling(_pollingFrequency));
    }

    public AristaConnector(double pollingFrequency)
    {
        _pollingFrequency = pollingFrequency;
        _pollingInterval = TimeSpan.FromSeconds(1 / _pollingFrequency);
        _pollingBuffer = new CircularBuffer<AristaJson>((int)Math.Ceiling(_pollingFrequency));
    }

    /// <summary>
    /// Retrieves the calculated results derived from the collected Arista polling data.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="AristaResults"/> that contains network statistics such as
    /// total RX and TX bit rates, as well as per-NIC RX and TX values.
    /// If the polling buffer is empty, returns null.
    /// </returns>
    public AristaResults? GetResults()
    {
        if (_pollingBuffer.IsEmpty)
        {
            return null;
        }
        
        var res = AristaResults.CreateNew(_pollingBuffer.Last());

        var rxSum = new Dictionary<string, long>();
        var txSum = new Dictionary<string, long>();
        
        foreach (var json in _pollingBuffer)
        {
            foreach (var nic in json.Nics)
            {
                // Sum RX Per Nic
                if (!rxSum.TryAdd(nic.MAC, nic.Rx))
                {
                    rxSum[nic.MAC] += nic.Rx;
                }
                // Sum TX Per Nic
                if (!txSum.TryAdd(nic.MAC, nic.Tx))
                {
                    txSum[nic.MAC] += nic.Tx;
                }
            }
        }

        foreach (var nicRes in res.Nics)
        {
            nicRes.RxBps = OctetsToBits((long)Math.Ceiling(rxSum[nicRes.MAC] / _pollingFrequency));
            nicRes.TxBps = OctetsToBits((long)Math.Ceiling(txSum[nicRes.MAC] / _pollingFrequency));
        }

        res.TotalRxBps = res.Nics.Sum(x => x.RxBps);
        res.TotalTxBps = res.Nics.Sum(x => x.TxBps);

        return res;
    }

    /// <summary>
    /// Asynchronously polls data from the configured data source and adds the parsed result
    /// to the internal polling buffer. The raw data is also logged to the console for visibility.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. This method does not return a result directly.
    /// </returns>
    private async Task InternalPollDataAsync()
    {
        
        // This is async for demo purposes, as most real data sources should be async as you'd be waiting
        // for network responses or similar and don't want to hold up the executing threads. 
        var dataString = await DataSource.GetDataAsync();
        
        // To test with multiple nic's this can be swaped out below, in a real world example this would be a 
        // good candidate to refactor into something that could be injected via Dependency injection allowing
        // for the same polling and calculation method to be used, while swapping out the source data.
        // dataString = await DataSource.GetMultiNicDataAsync();
        
        // Deserialise and add to the poll buffer.
        var data = JsonSerializer.Deserialize<AristaJson>(dataString, _defaultSerializerOptions);
        if(data is not null)
            _pollingBuffer.Add(data);

        // Easy way to see that it's polling as required not required for function. 
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(dataString);
        Console.ForegroundColor = ConsoleColor.White;
    }


    /// <summary>
    /// Periodically polls data from the connected Arista device at fixed intervals.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests. If triggered, stops the polling process gracefully.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation. The task completes
    /// once the polling loop is terminated due to cancellation or an unhandled exception.
    /// </returns>
    public async Task PollPeriodicAsync(CancellationToken cancellationToken = default)
    {
        using PeriodicTimer timer = new(_pollingInterval);
        while (true)
        {
            await InternalPollDataAsync();
            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    
    private static long OctetsToBits(long source) => source * 8;

}