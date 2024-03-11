using Core.Models.Configuration.Sinks;
using HomeAssistantSink.Models;
using PingTest.Models;

namespace HomeAssistantSink.Helpers;

public class PingTestRecordDataHelper
{
	private readonly PingTestResult _pingTestResult;
	private readonly SinkAmountType _sinkAmountType;

	public PingTestRecordDataHelper(PingTestResult pingTestResult,  SinkAmountType sinkAmountType)
	{
		_pingTestResult = pingTestResult;
		_sinkAmountType = sinkAmountType;
	}
	
	public List<EntityDataRecord> GetRecords()
	{
		var records = new List<EntityDataRecord>();
		var baseEntityId = "sensor.ispsnitch_ping_";
		
		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			records.Add( new EntityDataRecord { EntityId = $"{baseEntityId}online", EntityData = new EntityData { State = _pingTestResult.IsSuccess.ToString()}});
			records.Add( new EntityDataRecord { EntityId = $"{baseEntityId}roundtrip_time", EntityData = new EntityData { State = _pingTestResult.RoundtripTime.ToString(), Attributes = new Dictionary<string, string>{{"unit_of_measurement", "ms"}}}});
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			records.Add( new EntityDataRecord { EntityId = $"{baseEntityId}address", EntityData = new EntityData { State = _pingTestResult.Address}});
			records.Add( new EntityDataRecord { EntityId = $"{baseEntityId}status", EntityData = new EntityData { State = _pingTestResult.Status}});
			records.Add( new EntityDataRecord { EntityId = $"{baseEntityId}response-ip", EntityData = new EntityData { State = _pingTestResult.ResponseIp}});
		}
		
		return records;
	}
}