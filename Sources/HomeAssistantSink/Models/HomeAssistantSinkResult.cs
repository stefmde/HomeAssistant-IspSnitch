using System.Collections.Generic;
using Core.Models.Configuration.Sinks;

namespace HomeAssistantSink.Models;

public class HomeAssistantSinkResult : ISinkBaseResult
{
	public bool Success { get; set; }
	
	public List<string> FailedEntityIds { get; set; }
	
	public HomeAssistantSinkResultErrorType ErrorType { get; set; }
}