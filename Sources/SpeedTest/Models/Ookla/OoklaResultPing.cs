
namespace SpeedTest.Models.Ookla;

public class OoklaResultPing
{
	public float Jitter { get; set; }
	
	public float Latency { get; set; }
	
	public float Low { get; set; }
	
	public float High { get; set; }
	
	public override string ToString()
	{
		return $"Ping: {Latency} ms (jitter: {Jitter} ms, low: {Low} ms, high: {High} ms)";
	}
}