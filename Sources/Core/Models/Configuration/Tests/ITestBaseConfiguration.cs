namespace Core.Models.Configuration.Tests;

public interface ITestBaseConfiguration : IBaseConfiguration
{
	public int SecondsBetween { get; set; }
	
	public bool WriteInfluxDbEnabled { get; set; }
	
	public bool WriteToHomeAssistantEnabled { get; set; }
	
	public int WriteToHomeAssistantEverySeconds { get; set; }
}