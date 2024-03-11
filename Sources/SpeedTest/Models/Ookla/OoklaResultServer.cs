
namespace SpeedTest.Models.Ookla;

public class OoklaResultServer
{
	public int Id { get; set; }
	
	public string Host { get; set; }
	
	public int Port { get; set; }
	
	public string Name { get; set; }
	
	public string Location { get; set; }
	
	public string Country { get; set; }
	
	public string Ip { get; set; }

	public override string ToString()
	{
		return $"Server: {Name} {Location}, Id: {Id}";
	}
}