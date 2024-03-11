using Core.Models.Configuration.Sinks;
using InfluxDB.Client.Writes;
using SpeedTest.Models;
using SpeedTest.Models.Ookla;

namespace InfluxDbSink.Helpers;

public class SpeedTestPointDataHelper
{
	private readonly SpeedTestResult _speedTestResult;
	private readonly SinkAmountType _sinkAmountType;
	private readonly PointDataHelper _pointDataHelper;

	public SpeedTestPointDataHelper(SpeedTestResult speedTestResult, SinkAmountType sinkAmountType, string source)
	{
		_speedTestResult = speedTestResult;
		_sinkAmountType = sinkAmountType;
		_pointDataHelper = new PointDataHelper("speedtest", source, _speedTestResult.CreatedUtc);
	}
	
	public List<PointData> GetPoints()
	{
		var points = new List<PointData>();
		
		points.Add(_pointDataHelper.CreatePoint("online", _speedTestResult.IsSuccess));
		
		if (!_speedTestResult.IsSuccess)
		{
			return points;
		}
		
		if (_speedTestResult.OoklaResult is null)
		{
			return points;
		}
		
		points.AddRange(GetRootPoints());
		points.AddRange(GetPingPoints());
		points.AddRange(GetDirectionPoints("download", _speedTestResult.OoklaResult.Download));
		points.AddRange(GetDirectionPoints("upload", _speedTestResult.OoklaResult.Upload));
		points.AddRange(GetInterfacePoints());
		points.AddRange(GetServerPoints());
		
		return points;
	}

	private List<PointData> GetRootPoints()
	{
		var points = new List<PointData>();

		if (_speedTestResult.OoklaResult.Ping is null)
		{
			return points;
		}

		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint("package-loss-percentage", _speedTestResult.OoklaResult.PacketLoss));
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("isp", _speedTestResult.OoklaResult.Isp));
		}

		return points;
	}

	private List<PointData> GetPingPoints()
	{
		var points = new List<PointData>();

		if (_speedTestResult.OoklaResult.Ping is null)
		{
			return points;
		}

		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint("ping-ms", _speedTestResult.OoklaResult.Ping.Latency));
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("ping-jitter-ms", _speedTestResult.OoklaResult.Ping.Jitter));
		}

		if (_sinkAmountType >= SinkAmountType.Full)
		{
			points.Add(_pointDataHelper.CreatePoint("ping-low-ms", _speedTestResult.OoklaResult.Ping.Low));
			points.Add(_pointDataHelper.CreatePoint("ping-high-ms", _speedTestResult.OoklaResult.Ping.High));
		}

		return points;
	}
	
	private List<PointData> GetDirectionPoints(string direction, OoklaResultTestDirection data)
	{
		var points = new List<PointData>();

		if (data is null)
		{
			return points;
		}

		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint($"{direction}-bandwidth-bytes", data.Bandwidth.ToString()));
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint($"{direction}-latency-iqm-ms", data.Latency.Iqm));
			points.Add(_pointDataHelper.CreatePoint($"{direction}-latency-jitter-ms", data.Latency.Jitter));
		}

		if (_sinkAmountType >= SinkAmountType.Full)
		{
			points.Add(_pointDataHelper.CreatePoint($"{direction}-bytes-transferred", data.Bytes.ToString()));
			points.Add(_pointDataHelper.CreatePoint($"{direction}-elapsed-ms", data.Elapsed.ToString()));
			points.Add(_pointDataHelper.CreatePoint($"{direction}-latency-low", data.Latency.Low));
			points.Add(_pointDataHelper.CreatePoint($"{direction}-latency-high", data.Latency.High));
		}

		return points;
	}

	private List<PointData> GetInterfacePoints()
	{
		var points = new List<PointData>();

		if (_speedTestResult.OoklaResult.Interface is null)
		{
			return points;
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("interface-name", _speedTestResult.OoklaResult.Interface.Name));
		}

		if (_sinkAmountType >= SinkAmountType.Full)
		{
			points.Add(_pointDataHelper.CreatePoint("interface-internal-ip", _speedTestResult.OoklaResult.Interface.InternalIp));
			points.Add(_pointDataHelper.CreatePoint("interface-external-ip", _speedTestResult.OoklaResult.Interface.ExternalIp));
			points.Add(_pointDataHelper.CreatePoint("interface-mac-address", _speedTestResult.OoklaResult.Interface.MacAddr));
			points.Add(_pointDataHelper.CreatePoint("interface-is-vpn", _speedTestResult.OoklaResult.Interface.IsVpn));
		}

		return points;
	}
	
	private List<PointData> GetServerPoints()
	{
		var points = new List<PointData>();

		if (_speedTestResult.OoklaResult.Server is null)
		{
			return points;
		}

		if (_sinkAmountType >= SinkAmountType.Basic)
		{
			points.Add(_pointDataHelper.CreatePoint("server-location", _speedTestResult.OoklaResult.Server.Location));
			points.Add(_pointDataHelper.CreatePoint("server-country", _speedTestResult.OoklaResult.Server.Country));
		}

		if (_sinkAmountType >= SinkAmountType.Extended)
		{
			points.Add(_pointDataHelper.CreatePoint("server-name", _speedTestResult.OoklaResult.Server.Name));
		}

		if (_sinkAmountType >= SinkAmountType.Full)
		{
			points.Add(_pointDataHelper.CreatePoint("server-id", _speedTestResult.OoklaResult.Server.Id));
			points.Add(_pointDataHelper.CreatePoint("server-host", _speedTestResult.OoklaResult.Server.Host));
			points.Add(_pointDataHelper.CreatePoint("server-port", _speedTestResult.OoklaResult.Server.Port));
			points.Add(_pointDataHelper.CreatePoint("server-ip", _speedTestResult.OoklaResult.Server.Ip));
		}

		return points;
	}
}