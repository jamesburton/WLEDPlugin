using Kevsoft.WLED;
using Zeroconf;

public static class WLEDDevices
{
    public static async Task<Dictionary<string, string>> DetectWLEDDevices(int timeoutSeconds = 3)
    {
        IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_http._tcp.local.", scanTime: TimeSpan.FromSeconds(timeoutSeconds));

        var wledIPDictionary = new Dictionary<string, string>();

        foreach (var host in results)
        {
            Console.WriteLine($"[{host.DisplayName}]");
            foreach (var address in host.IPAddresses)
            {
                Console.WriteLine($"\t{address}");
            }

            var client = new WLedClient($"http://{host.IPAddress}/");

            // Verify it is a WLED device by calling the API
            try
            {
                // var data = await client.Get();
                var data = await client.GetInformation();
                // Console.WriteLine($"WLED State: {JsonSerializer.Serialize(data.State)}");
                wledIPDictionary.Add(host.DisplayName, host.IPAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Console.WriteLine("WLED IP Dictionary:");
        // Console.WriteLine(JsonSerializer.Serialize(wledIPDictionary));

        return wledIPDictionary;
    }

    public static async Task ToggleLights(Dictionary<string, string> wledIPDictionary, bool on)
    {
        foreach (var ip in wledIPDictionary.Values)
        {
            var client = new WLedClient($"http://{ip}/");
            await client.Post(new StateRequest { On = on });
        }
    }

    public static async Task ToggleLights(string ip, bool on)
    {
        var client = new WLedClient($"http://{ip}/");
        await client.Post(new StateRequest { On = on });
    }

    public static async Task SetBrightness(string ip, byte brightness)
    {
        var client = new WLedClient($"http://{ip}/");
        await client.Post(new StateRequest { Brightness = brightness });
    }

    public static async Task<string[]> GetEffects(string ip)
    {
        var client = new WLedClient($"http://{ip}/");
        return await client.GetEffects();
    }

    public static async Task<byte> GetBrightness(string ip)
    {
        var client = new WLedClient($"http://{ip}/");
        var data = await client.GetState();
        return data.Brightness;
    }

    public static async Task<int> GetCurrentPreset(string ip)
    {
        var client = new WLedClient($"http://{ip}/");
        var data = await client.GetState();
        return data.PresetId;
    }

    public static async Task SetPreset(string ip, int presetId)
    {
        var client = new WLedClient($"http://{ip}/");
        await client.Post(new StateRequest { PresetId = presetId });
    }
}