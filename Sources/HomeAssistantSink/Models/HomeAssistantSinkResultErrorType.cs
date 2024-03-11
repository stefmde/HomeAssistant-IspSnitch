namespace HomeAssistantSink.Models;

public enum HomeAssistantSinkResultErrorType
{
	None,
	SkippedDueToGlobalDisabled,
	ErrorApi,
	ErrorTokenNotSet
}