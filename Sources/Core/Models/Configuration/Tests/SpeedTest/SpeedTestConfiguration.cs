namespace Core.Models.Configuration.Tests.SpeedTest;

public class SpeedTestConfiguration : ITestBaseConfiguration
{
	public bool Enabled { get; set; }
	
	public bool Debug { get; set; }

	public int SecondsBetween { get; set; }
	
	public bool WriteInfluxDbEnabled { get; set; }
	
	public bool WriteToHomeAssistantEnabled { get; set; }
	
	public int WriteToHomeAssistantEverySeconds { get; set; }
	
	public int ForceServerById { get; set; } = 0;
	
	public string ForceServerByHostName { get; set; }
	
	public string ForceInterfaceByName { get; set; }
}