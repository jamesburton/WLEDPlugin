// Based on Semantic Kernal light plugin: https://learn.microsoft.com/en-us/semantic-kernel/overview/#how-many-agents-does-it-take-to-change-a-lightbulb
// Plus uses the WLED API: https://github.com/kevbite/WLED.NET
// And detects WLED devices on the network using mDNS using Zeroconf: https://github.com/novotnyllc/Zeroconf

using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace WLEDPlugin;

public class WLEDPlugin
{
    public Dictionary<string, string> DeviceIPsByName { get; set; } = new Dictionary<string, string>();

    [KernelFunction]
    [Description("Detects the WLED devices on the network")]
    public async Task DetectWLEDDevices()
    {
        var wledDevices = await WLEDDevices.DetectWLEDDevices();
        foreach (var device in wledDevices)
        {
            Console.WriteLine($"Found WLED device: {device.Key} at {device.Value}");
        }
        DeviceIPsByName = wledDevices;
    }

    [KernelFunction]
    [Description("Turns on the WLED devices on the network")]
    public async Task TurnOnWLEDDevices()
    {
        if(DeviceIPsByName.Count == 0)
            await DetectWLEDDevices();

        if(DeviceIPsByName.Count == 0)
            throw new Exception("No WLED devices found on the network");

        await WLEDDevices.ToggleLights(DeviceIPsByName, true);

        Console.WriteLine($"[{DeviceIPsByName.Count} Light(s) now on]");
    }

    [KernelFunction]
    [Description("Turns off the WLED devices on the network")]
    public async Task TurnOffWLEDDevices()
    {
        if(DeviceIPsByName.Count == 0)
            await DetectWLEDDevices();

        if(DeviceIPsByName.Count == 0)
            throw new Exception("No WLED devices found on the network");

        await WLEDDevices.ToggleLights(DeviceIPsByName, false);

        Console.WriteLine($"[{DeviceIPsByName.Count} Light(s) now off]");
    }
}
