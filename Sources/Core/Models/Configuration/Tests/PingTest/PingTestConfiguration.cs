namespace Core.Models.Configuration.Tests.PingTest;

public class PingTestConfiguration : ITestBaseConfiguration
{
	public bool Enabled { get; set; }
	
	public bool Debug { get; set; }

	public int SecondsBetween { get; set; }
	
	public bool WriteInfluxDbEnabled { get; set; }
	
	public bool WriteToHomeAssistantEnabled { get; set; }
	
	public int WriteToHomeAssistantEverySeconds { get; set; }
	
	public string Address { get; set; }
	
	public int TimeoutMs { get; set; }
}