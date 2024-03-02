using System.Text;

namespace Speedtest.Models.Ookla;

public class OoklaResultTestDirection
{
	public long Bandwidth { get; set; }
	
	public int Bytes { get; set; }
	
	public int Elapsed { get; set; }
	
	public OoklaResultLatency? Latency { get; set; }
	
	public override string ToString()
	{
		var resultBuilder = new StringBuilder();
		resultBuilder.AppendLine($"{Bandwidth} (data used: {Bytes} Bytes)");
		resultBuilder.Append("     ");
		resultBuilder.Append(Latency);
		return resultBuilder.ToString();
	}
}