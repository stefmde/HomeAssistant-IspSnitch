// See https://aka.ms/new-console-template for more information


using Core.Helper;
using IspSnitch.Helper;
using Speedtest;

Console.WriteLine("ISP Snitch");
Console.WriteLine();
Console.WriteLine("init...");
var ispConfig = await ConfigurationHelper.GetConfiguration();
var storage = new Storage.Storage(ispConfig.StorageConfiguration);
var pingTest = new PingTest.PingTest(ispConfig.PingTestConfiguration);
var speedTest = new SpeedTest(ispConfig.SpeedTestConfiguration);
Console.WriteLine("init... done.");
Console.WriteLine("Start Testing");
Console.WriteLine();

async Task PingTest()
{
	if (ispConfig.PingTestConfiguration.Enabled)
	{
		var config = ispConfig.PingTestConfiguration;
		Console.WriteLine($"Starting PingTests to '{config.Address}' with Timeout '{config.TimeoutMs}', SecondsBetween '{config.SecondsBetween}' and Debug '{config.Debug}'");
		while (true)
		{
			if(config.Debug) Console.WriteLine("PingTesting... ");
			var pingTestResult = await pingTest.Test();
			if (pingTestResult.IsSuccess)
			{
				Console.WriteLine($"Ping {pingTestResult.RoundtripTime} ms");
				await storage.StorePingTestData(pingTestResult);
			}
			else
			{
				Console.WriteLine( $"Ping FAILED: {pingTestResult.Status}" );
			}
			
			Thread.Sleep(config.SecondsBetween * 1000);
		}
	}
	Console.WriteLine("PingTests NOT enabled");
}

async Task SpeedTest()
{
	if (ispConfig.SpeedTestConfiguration.Enabled)
	{
		var config = ispConfig.SpeedTestConfiguration;
		Console.WriteLine($"Starting SpeedTests with MinutesBetween '{config.MinutesBetween}' and Debug '{config.Debug}'");
		while (true)
		{
			if(config.Debug) Console.WriteLine("SpeedTesting...");
			var speedtestResult = await speedTest.Test();

			if (speedtestResult.IsSuccess)
			{
				if (speedtestResult.OoklaResult is not null)
				{
					Console.WriteLine($"SpeedTest {speedtestResult.OoklaResult.Ping.Latency} ms; Down {speedtestResult.OoklaResult?.Download.Bandwidth.ToReadableSpeedUnit()}; Up {speedtestResult.OoklaResult.Upload.Bandwidth.ToReadableSpeedUnit()}" );
					await storage.StoreSpeedTestData(speedtestResult);
				}
			}
			else
			{
				Console.WriteLine($"SpeedTest FAILED: {speedtestResult.Error}");
			}
			
			Thread.Sleep( config.MinutesBetween * 60 * 1000);
		}
	}
	Console.WriteLine("SpeedTests NOT enabled");
}

var pingTestFunc = PingTest();
var speedTestFunc = SpeedTest();

await Task.WhenAll(pingTestFunc, speedTestFunc);