namespace Core.Models.Configuration.Sinks.HomeAssistantSink;

public class HomeAssistantSinkConfiguration : ISinkBaseConfiguration
{
	public bool Enabled { get; set; }
	
	public bool Debug { get; set; }
	
	public SinkAmountType SinkAmountType { get; set; }
}