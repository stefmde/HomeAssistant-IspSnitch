using Core.Models.Configuration.Storage;
using InfluxDB.Client.Writes;
using PingTest.Models;
using Speedtest.Models;
using Speedtest.Models.Ookla;

namespace Storage.Helpers;

public class PingTestPointDataHelper
{
	private readonly PingTestResult _pingTestResult;
	private readonly StoreType _storeType;
	private readonly PointDataHelper _pointDataHelper;

	public PingTestPointDataHelper(PingTestResult pingTestResult,  StoreType storeType, string source)
	{
		_pingTestResult = pingTestResult;
		_storeType = storeType;
		_pointDataHelper = new PointDataHelper("pingtest", source, _pingTestResult.CreatedUtc);
	}
	
	public List<PointData> GetPoints()
	{
		var points = new List<PointData>();
		
		if (_storeType >= StoreType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint("online", _pingTestResult.IsSuccess));
			points.Add(_pointDataHelper.CreatePoint("roundtrip-ms", _pingTestResult.RoundtripTime));
		}

		if (_storeType >= StoreType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("address", _pingTestResult.Address));
			points.Add(_pointDataHelper.CreatePoint("status", _pingTestResult.Status));
			points.Add(_pointDataHelper.CreatePoint("response-ip", _pingTestResult.ResponseIp));
		}
		
		return points;
	}
}