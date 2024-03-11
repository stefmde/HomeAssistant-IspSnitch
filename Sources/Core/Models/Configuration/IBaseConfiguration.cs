namespace Core.Models.Configuration;

public interface IBaseConfiguration
{
	public bool Enabled { get; set; }
	
	public bool Debug { get; set; }
}