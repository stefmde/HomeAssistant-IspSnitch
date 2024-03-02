// See https://aka.ms/new-console-template for more information

using Core.Models.Configuration;
using Core.Models.Configuration.Pingtest;
using Core.Models.Configuration.Speedtest;
using Core.Models.Configuration.Storage;

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
		Debug = true,
		Enabled = true,
		Address = "google.com",
		TimeoutMs = 10000
	},
	SpeedTestConfiguration = new SpeedTestConfiguration
	{
		Debug = true,
		Enabled = true
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

var storage = new Storage.Storage(config.StorageConfiguration);

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