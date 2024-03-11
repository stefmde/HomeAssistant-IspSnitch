using System.Collections.Generic;
using Core.Models.Configuration.Sinks;
using InfluxDB.Client.Writes;
using PingTest.Models;

namespace InfluxDbSink.Helpers;

public class PingTestPointDataHelper
{
	private readonly PingTestResult _pingTestResult;
	private readonly SinkAmountType _sinkAmountType;
	private readonly PointDataHelper _pointDataHelper;

	public PingTestPointDataHelper(PingTestResult pingTestResult,  SinkAmountType sinkAmountType, string source)
	{
		_pingTestResult = pingTestResult;
		_sinkAmountType = sinkAmountType;
		_pointDataHelper = new PointDataHelper("pingtest", source, _pingTestResult.CreatedUtc);
	}
	
	public List<PointData> GetPoints()
	{
		var points = new List<PointData>();
		
		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint("online", _pingTestResult.IsSuccess));
			points.Add(_pointDataHelper.CreatePoint("roundtrip-ms", _pingTestResult.RoundtripTime));
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("address", _pingTestResult.Address));
			points.Add(_pointDataHelper.CreatePoint("status", _pingTestResult.Status));
			points.Add(_pointDataHelper.CreatePoint("response-ip", _pingTestResult.ResponseIp));
		}
		
		return points;
	}
}