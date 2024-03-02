using Core.Models.Configuration.Pingtest;
using Core.Models.Configuration.Speedtest;
using Core.Models.Configuration.Storage;

namespace Core.Models.Configuration;

public class IspSnitchConfiguration
{
	public SpeedTestConfiguration SpeedTestConfiguration { get; set; }
	
	public PingTestConfiguration PingTestConfiguration { get; set; }
	
	public StorageConfiguration StorageConfiguration { get; set; }
}