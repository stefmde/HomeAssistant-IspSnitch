namespace SpeedTest.Models.Ookla;

public class OoklaResultInterface
{
	public string InternalIp { get; set; }
	
	public string Name { get; set; }
	
	public string MacAddr { get; set; }
	
	public bool IsVpn { get; set; }
	
	public string ExternalIp { get; set; }
}