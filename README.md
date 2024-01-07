# WLEDPlugin

This is a plugin for use with Microsoft.SemanticKernel, written in .NET.

It currently allows detecting WLED devices on your local network via mDNS and then turning them on and off.  It is intended to extend this with more API functions to control brightness, apply presets, etc.

## Usage

* Ensure you have an OpenAI API account, and add your API Key as an environment variable called `OPENAI_API_KEY`
* Run the `WLEDPlugin.Console` project and chat with the agent

## References

* Creating AI agents with semantic kernel: https://learn.microsoft.com/en-us/semantic-kernel/overview/
* WLED Nuget Package: https://github.com/kevbite/WLED.NET
* Zeroconf mDNS device detection: https://github.com/novotnyllc/Zeroconf
