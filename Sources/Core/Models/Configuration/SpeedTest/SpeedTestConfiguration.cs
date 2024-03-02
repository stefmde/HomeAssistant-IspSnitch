namespace Core.Models.Configuration.Speedtest;

public class SpeedTestConfiguration
{
	public bool Debug { get; set; }
	
	public bool Enabled { get; set; }

	public int MinutesBetween { get; set; }
	
	public int ForceServerById { get; set; } = 0;
	
	public string ForceServerByHostName { get; set; }
	
	public string ForceInterfaceByName { get; set; }
}