
namespace Speedtest.Models.Ookla;

public class OoklaResultLatency
{
	public float Iqm { get; set; }
	
	public float Low { get; set; }
	
	public float High { get; set; }
	
	public float Jitter { get; set; }
	
	public override string ToString()
	{
		return $"{Iqm} ms (jitter: {Jitter} ms, low: {Low} ms, high: {High} ms)";
	}
}