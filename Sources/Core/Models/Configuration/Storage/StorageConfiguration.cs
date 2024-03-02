namespace Core.Models.Configuration.Storage;

public class StorageConfiguration
{
	public bool Debug { get; set; }
	
	public string Url { get; set; }
	
	public string Token { get; set; }
	
	public string UserName { get; set; }
	
	public string UserPassword { get; set; }
	
	public string Database { get; set; }
	
	public StoreType StoreType { get; set; }
	
	public string Source { get; set; }
}