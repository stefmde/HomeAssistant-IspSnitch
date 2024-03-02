namespace Storage.Models;

public class StoreResult
{
	public bool IsSuccess { get; set; }
	
	public string Error { get; internal set; }
	
	public string Debug { get; private set; }
	
	public int WrittenPointDataCount { get; internal set; }

	public void DebugAppend(string message)
	{
		Debug += $"{message}{Environment.NewLine}{Environment.NewLine}";
	}
}