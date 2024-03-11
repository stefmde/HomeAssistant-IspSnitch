using System;
using SpeedTest.Models.Ookla;

namespace SpeedTest.Models;

public class SpeedTestResult
{
	public OoklaResult? OoklaResult { get; internal set; }
	
	public DateTime CreatedUtc { get; private set; } = DateTime. UtcNow;
	
	public bool IsSuccess { get; internal set; }
	
	public string Error { get; internal set; }
	
	public string Debug { get; private set; }

	public void DebugAppend(string message)
	{
		Debug += $"{message}{Environment.NewLine}{Environment.NewLine}";
	}
}