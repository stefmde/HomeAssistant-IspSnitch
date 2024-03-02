using Core.Models.Configuration;
using Core.Models.Configuration.Pingtest;
using Core.Models.Configuration.Speedtest;
using Core.Models.Configuration.Storage;
using Newtonsoft.Json;

namespace IspSnitch.Helper;

public static class ConfigurationHelper
{
	public const string ConfigurationFile = "/data/options.json";

	public static async Task<IspSnitchConfiguration> GetConfiguration()
	{
		if (File.Exists(ConfigurationFile))
		{
			return await ParseConfiguration();
		}

		return await StaticConfiguration();
	}
	
	public static async Task<IspSnitchConfiguration> StaticConfiguration()
	{
		Console.Write("Using static config... ");
		var config = new IspSnitchConfiguration
		{
			PingTestConfiguration = new PingTestConfiguration
			{
				Debug = false,
				Enabled = false,
				Address = "google.com",
				TimeoutMs = 10000,
				SecondsBetween = 1
			},
			SpeedTestConfiguration = new SpeedTestConfiguration
			{
				Debug = false,
				Enabled = true,
				MinutesBetween = 2
			},
			StorageConfiguration = new StorageConfiguration
			{
				Debug = true,
				Url = "http://192.168.1.12:8086",
				Token = "",
				UserName = "isp-snitch-dev-user",
				UserPassword = "isp-snitch-dev-user",
				Database = "isp-snitch-dev-db",
				Source = "Dev",
				StoreType = StoreType.Full
			}
		};
		Console.WriteLine("done.");

		return config;
	}
	
	public static async Task<IspSnitchConfiguration> ParseConfiguration()
	{
		Console.Write("Reading config... ");
		if (!File.Exists(ConfigurationFile))
		{
			throw new FileNotFoundException($"Configuration file '{ConfigurationFile}' not found");
		}

		var json = await File.ReadAllTextAsync(ConfigurationFile);
		var config = JsonConvert.DeserializeObject<IspSnitchConfiguration>(json);
		Console.WriteLine("done.");
		
		return config;
	}
}