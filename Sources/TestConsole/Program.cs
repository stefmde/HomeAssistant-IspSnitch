// See https://aka.ms/new-console-template for more information

using Core.Models.Configuration;
using Core.Models.Configuration.Sinks;
using Core.Models.Configuration.Sinks.HomeAssistantSink;
using Core.Models.Configuration.Sinks.InfluxDbSink;
using Core.Models.Configuration.Tests.PingTest;
using Core.Models.Configuration.Tests.SpeedTest;

/*
 * Database: isp-snitch-dev-db
 * Connection: isp-snitch-dev-connection
 * User: isp-snitch-dev-user
 * Password: isp-snitch-dev-user
 * 
 */

var config = new IspSnitchConfiguration
{
	PingTestConfiguration = new PingTestConfiguration
	{
		Enabled = true,
		Debug = true,
		Address = "google.com",
		TimeoutMs = 10000
	},
	SpeedTestConfiguration = new SpeedTestConfiguration
	{
		Enabled = true,
		Debug = true,
	},
	InfluxDbSinkConfiguration = new InfluxDbSinkConfiguration
	{
		Enabled = true,
		Debug = true,
		Url = "http://192.168.1.12:8086",
		Token = "",
		UserName = "isp-snitch-dev-user",
		UserPassword = "isp-snitch-dev-user",
		Database = "isp-snitch-dev-db",
		Source = "Dev",
		SinkAmountType = SinkAmountType.Full
	},
	HomeAssistantSinkConfiguration = new HomeAssistantSinkConfiguration
	{
		Enabled = true,
		Debug = true
	}
};

var storage = new InfluxDbSink.InfluxDbSink(config.InfluxDbSinkConfiguration);

var pingtest = new PingTest.PingTest(config.PingTestConfiguration);
var pingtestResult = await pingtest.Test();
await storage.StorePingTestData(pingtestResult);

// var speedtest = new Speedtest.SpeedTest(config.SpeedTestConfiguration);
// var speedtestResult = await speedtest.Test();
// await storage.StoreSpeedTestData(speedtestResult); 

// select count(*) from /.*/ 
// DROP SERIES FROM /.*/;

storage.Dispose();


Console.WriteLine();