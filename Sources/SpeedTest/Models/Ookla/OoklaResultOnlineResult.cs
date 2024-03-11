using System;

namespace SpeedTest.Models.Ookla;

public class OoklaResultOnlineResult
{
	public Guid Id { get; set; }
	
	public string Url { get; set; }
	
	public bool Persisted { get; set; }
}