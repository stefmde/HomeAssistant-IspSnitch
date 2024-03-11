// See https://aka.ms/new-console-template for more information


using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Helper;
using IspSnitch.Helper;

Console.WriteLine("ISP Snitch");
Console.WriteLine();
Console.WriteLine("init...");
var ispConfig = await ConfigurationHelper.GetConfiguration();
var influxDbSink = new InfluxDbSink.InfluxDbSink(ispConfig.InfluxDbSinkConfiguration);
var homeAssistantSink = new HomeAssistantSink.HomeAssistantSink(ispConfig.HomeAssistantSinkConfiguration);
var pingTest = new PingTest.PingTest(ispConfig.PingTestConfiguration);
var speedTest = new SpeedTest.SpeedTest(ispConfig.SpeedTestConfiguration);
Console.WriteLine("init... done.");
Console.WriteLine("Start Testing");
Console.WriteLine();

async Task PingTest()
{
	if (ispConfig.PingTestConfiguration.Enabled)
	{
		var config = ispConfig.PingTestConfiguration;
		var nextHomeAssistantSinkStore = DateTime.MinValue;
		Console.WriteLine($"Starting PingTests to '{config.Address}' with Timeout '{config.TimeoutMs}', SecondsBetween '{config.SecondsBetween}' and Debug '{config.Debug}'");
		while (true)
		{
			// TEST
			if(config.Debug) Console.WriteLine("PingTesting... ");
			var testResult = await pingTest.Test();
			if (testResult.IsSuccess)
			{
				Console.WriteLine($"Ping {testResult.RoundtripTime} ms");
				
				// STORE - InfluxDB
				if (config.WriteInfluxDbEnabled)
				{
					await influxDbSink.StorePingTestData(testResult);
				}
				
				// STORE - Home Assistant
				if (config.WriteToHomeAssistantEnabled && nextHomeAssistantSinkStore < DateTime.UtcNow)
				{
					await homeAssistantSink.StorePingTestData(testResult);
					nextHomeAssistantSinkStore = DateTime.UtcNow.AddSeconds(config.WriteToHomeAssistantEverySeconds);
				}
			}
			else
			{
				Console.WriteLine( $"Ping FAILED: {testResult.Status}" );
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
		Console.WriteLine($"Starting SpeedTests with SecondsBetween '{config.SecondsBetween}' and Debug '{config.Debug}'");
		while (true)
		{
			if(config.Debug) Console.WriteLine("SpeedTesting...");
			var testResult = await speedTest.Test();

			if (testResult.IsSuccess)
			{
				if (testResult.OoklaResult is not null)
				{
					Console.WriteLine($"SpeedTest {testResult.OoklaResult.Ping.Latency} ms; Down {testResult.OoklaResult?.Download.Bandwidth.ToReadableSpeedUnit()}; Up {testResult.OoklaResult.Upload.Bandwidth.ToReadableSpeedUnit()}" );
					await influxDbSink.StoreSpeedTestData(testResult);
				}
			}
			else
			{
				Console.WriteLine($"SpeedTest FAILED: {testResult.Error}");
			}
			
			Thread.Sleep( config.SecondsBetween * 60 * 60 * 1000);
		}
	}
	Console.WriteLine("SpeedTests NOT enabled");
}

var pingTestFunc = PingTest();
var speedTestFunc = SpeedTest();

await Task.WhenAll(pingTestFunc, speedTestFunc);