namespace Core.Models.Configuration.Sinks.InfluxDbSink;

public class InfluxDbSinkConfiguration : ISinkBaseConfiguration
{
	public bool Debug { get; set; }
	
	public bool Enabled { get; set; }
	
	public SinkAmountType SinkAmountType { get; set; }

	public string Url { get; set; }
	
	public string Token { get; set; }
	
	public string UserName { get; set; }
	
	public string UserPassword { get; set; }
	
	public string Database { get; set; }
	
	public string Source { get; set; }
}