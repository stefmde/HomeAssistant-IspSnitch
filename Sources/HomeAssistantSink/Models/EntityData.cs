using System.Collections.Generic;

namespace HomeAssistantSink.Models;

public class EntityData
{
	public string State { get; set; }

	public Dictionary<string, string> Attributes = new ();
}