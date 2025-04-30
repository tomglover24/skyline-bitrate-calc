using System.Text.Json;

namespace skyline_bitrate_calc_tom_glover;

class Program
{
    /// <summary>
    /// Polling Frequency in Hz
    /// </summary>
    private const double PollingFrequency = 2.0;
    static async Task Main(string[] args)
    {
        Console.WriteLine("------");
        Console.WriteLine("Press Control + C to Exit;");

        // Init the demo connector
        var connector = new AristaConnector(PollingFrequency);

        // Yep this is not the best way to trigger a periodic poll, but it's the easiest and quickest way for demo purposes.
        var cts = new CancellationTokenSource();
        _ = connector.PollPeriodicAsync(cts.Token);

        while (true)
        {
            await Task.Delay(2000);
            // Poll for Results from the connector, and serialise just for ease to read.
            Console.WriteLine(JsonSerializer.Serialize(connector.GetResults()));
        }
    }
}