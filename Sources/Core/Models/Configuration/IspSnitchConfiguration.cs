using Core.Models.Configuration.Sinks.HomeAssistantSink;
using Core.Models.Configuration.Sinks.InfluxDbSink;
using Core.Models.Configuration.Tests.PingTest;
using Core.Models.Configuration.Tests.SpeedTest;

namespace Core.Models.Configuration;

public class IspSnitchConfiguration
{
	public SpeedTestConfiguration SpeedTestConfiguration { get; set; }
	
	public PingTestConfiguration PingTestConfiguration { get; set; }
	
	public InfluxDbSinkConfiguration InfluxDbSinkConfiguration { get; set; }
	
	public HomeAssistantSinkConfiguration HomeAssistantSinkConfiguration { get; set; }
}