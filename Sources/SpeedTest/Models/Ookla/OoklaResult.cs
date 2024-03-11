using System;
using System.Text;

namespace SpeedTest.Models.Ookla;

public class OoklaResult
{
	public string Type { get; set; }
	
	public string Timestamp { get; set; }

	public DateTime? Time
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Timestamp))
			{
				return DateTime.Parse(Timestamp);
			}

			return null;
		}
	}

	public float PacketLoss { get; set; }
	
	public string Isp { get; set; }
	
	public OoklaResultPing? Ping { get; set; }
	
	public OoklaResultTestDirection? Download { get; set; }
	
	public OoklaResultTestDirection? Upload { get; set; }
	
	public OoklaResultInterface? Interface { get; set; }
	
	public OoklaResultServer? Server { get; set; }
	
	public OoklaResultOnlineResult? Result { get; set; }

	public override string ToString()
	{
		var resultBuilder = new StringBuilder();

		// Provider
		resultBuilder.AppendLine("Speedtest provided by Ookla, .Net Wrapper by StefmDE");
		resultBuilder.AppendLine();
		
		// Server
		resultBuilder.AppendLine(Server?.ToString());
		
		// ISP
		resultBuilder.Append("ISP: ");
		resultBuilder.AppendLine(Isp);
		
		// Ping
		resultBuilder.AppendLine();
		resultBuilder.AppendLine(Ping?.ToString());
		
		// Download
		resultBuilder.AppendLine();
		resultBuilder.Append("Download: ");
		resultBuilder.AppendLine(Download?.ToString());
		
		// Upload
		resultBuilder.AppendLine();
		resultBuilder.Append("Upload: ");
		resultBuilder.AppendLine(Download?.ToString());
		
		// Packet Loss
		resultBuilder.AppendLine();
		resultBuilder.Append("Packet Loss: ");
		resultBuilder.Append((PacketLoss * 100).ToString());
		resultBuilder.AppendLine(" %");
		
		// Result Url
		resultBuilder.Append("Result Url: ");
		resultBuilder.AppendLine(Result?.Url);

		return resultBuilder.ToString();
	}
}