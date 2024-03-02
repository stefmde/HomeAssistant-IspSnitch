namespace PingTest.Models;

public class PingTestResult
{
	public bool IsSuccess { get; set; }
	
	public string Status { get; set; }
	
	public string Address { get; set; }
	
	public string ResponseIp { get; set; }
	
	public long RoundtripTime { get; set; }
	
	public DateTime CreatedUtc { get; private set; } = DateTime.UtcNow;
}