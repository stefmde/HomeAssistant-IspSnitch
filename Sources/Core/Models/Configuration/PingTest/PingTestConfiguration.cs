using Core.Models.Configuration.Storage;

namespace Core.Models.Configuration.Pingtest;

public class PingTestConfiguration
{
	public bool Debug { get; set; }
	
	public bool Enabled { get; set; }
	
	public string Address { get; set; }
	
	public int TimeoutMs { get; set; }

	public int SecondsBetween { get; set; }
}