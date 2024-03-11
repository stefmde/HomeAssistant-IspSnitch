namespace Core.Models.Configuration.Sinks;

public interface ISinkBaseConfiguration : IBaseConfiguration
{
	public SinkAmountType SinkAmountType { get; set; }
}