using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Configuration.Sinks.InfluxDbSink;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using InfluxDbSink.Helpers;
using InfluxDbSink.Models;
using PingTest.Models;
using SpeedTest.Models;

namespace InfluxDbSink;

public class InfluxDbSink : IDisposable
{
	private readonly InfluxDbSinkConfiguration _influxDbSinkConfiguration;
	private readonly InfluxDBClient _influxClient;
	private readonly WriteApiAsync _influxWriteApi;

	// TODO disable storing if set so
	public InfluxDbSink(InfluxDbSinkConfiguration influxDbSinkConfiguration)
	{
		_influxDbSinkConfiguration = influxDbSinkConfiguration;

		if (string.IsNullOrWhiteSpace(_influxDbSinkConfiguration.Token))
		{
			_influxClient = new InfluxDBClient(_influxDbSinkConfiguration.Url, _influxDbSinkConfiguration.UserName, _influxDbSinkConfiguration.UserPassword, _influxDbSinkConfiguration.Database, "autogen");
		}
		else
		{
			_influxClient = new InfluxDBClient(_influxDbSinkConfiguration.Url, _influxDbSinkConfiguration.Token);
		}
		
		_influxWriteApi = _influxClient.GetWriteApiAsync();
	}

	public async Task<InfluxDbSinkResult> StorePingTestData(PingTestResult pingTestResult)
	{
		var pingTestPointDataHelper = new PingTestPointDataHelper(pingTestResult, _influxDbSinkConfiguration.SinkAmountType, _influxDbSinkConfiguration.Source);
		var points = pingTestPointDataHelper.GetPoints();
		var storeResult = await StoreGenericPointData(points);
		return storeResult;
	}

	public async Task<InfluxDbSinkResult> StoreSpeedTestData(SpeedTestResult speedTestResult)
	{
		var speedTestPointDataHelper = new SpeedTestPointDataHelper(speedTestResult, _influxDbSinkConfiguration.SinkAmountType, _influxDbSinkConfiguration.Source);
		var points = speedTestPointDataHelper.GetPoints();
		var storeResult = await StoreGenericPointData(points);
		return storeResult;
	}

	private async Task<InfluxDbSinkResult> StoreGenericPointData(List<PointData> points)
	{
		var storeResult = new InfluxDbSinkResult();
		try
		{
			if (_influxDbSinkConfiguration.Debug)
			{
				var result = await _influxWriteApi.WritePointsAsyncWithIRestResponse(points, _influxDbSinkConfiguration.Database, "ISP Snitch By StefmDE");
				storeResult.DebugAppend(result.ToString()); // TODO
			}
			else
			{
				await _influxWriteApi.WritePointsAsync(points, _influxDbSinkConfiguration.Database, "ISP Snitch By StefmDE");
			}

			storeResult.WrittenPointDataCount = points.Count;
			storeResult.Success = true;
		}
		catch (Exception ex)
		{
			storeResult.Error = ex.Message;
		}

		return storeResult;
	}
	
	public void Dispose()
	{
		_influxClient.Dispose();
	}
}