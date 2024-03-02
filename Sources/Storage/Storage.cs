using Core.Models.Configuration.Storage;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using PingTest.Models;
using Speedtest.Models;
using Storage.Helpers;
using Storage.Models;

namespace Storage;

public class Storage : IDisposable
{
	private readonly StorageConfiguration _storageConfiguration;
	private readonly InfluxDBClient _influxClient;
	private readonly WriteApiAsync _influxWriteApi;

	public Storage(StorageConfiguration storageConfiguration)
	{
		_storageConfiguration = storageConfiguration;

		if (string.IsNullOrWhiteSpace(_storageConfiguration.Token))
		{
			_influxClient = new InfluxDBClient(_storageConfiguration.Url, _storageConfiguration.UserName, _storageConfiguration.UserPassword, _storageConfiguration.Database, "autogen");
		}
		else
		{
			_influxClient = new InfluxDBClient(_storageConfiguration.Url, _storageConfiguration.Token);
		}
		
		_influxWriteApi = _influxClient.GetWriteApiAsync();
	}

	public async Task<StoreResult> StorePingTestData(PingTestResult pingTestResult)
	{
		var pingTestPointDataHelper = new PingTestPointDataHelper(pingTestResult, _storageConfiguration.StoreType, _storageConfiguration.Source);
		var points = pingTestPointDataHelper.GetPoints();
		var storeResult = await StoreGenericPointData(points);
		return storeResult;
	}

	public async Task<StoreResult> StoreSpeedTestData(SpeedTestResult speedTestResult)
	{
		var speedTestPointDataHelper = new SpeedTestPointDataHelper(speedTestResult, _storageConfiguration.StoreType, _storageConfiguration.Source);
		var points = speedTestPointDataHelper.GetPoints();
		var storeResult = await StoreGenericPointData(points);
		return storeResult;
	}

	private async Task<StoreResult> StoreGenericPointData(List<PointData> points)
	{
		var storeResult = new StoreResult();
		try
		{
			if (_storageConfiguration.Debug)
			{
				var result = await _influxWriteApi.WritePointsAsyncWithIRestResponse(points, _storageConfiguration.Database, "ISP Snitch By StefmDE");
				storeResult.DebugAppend(result.ToString()); // TODO
			}
			else
			{
				await _influxWriteApi.WritePointsAsync(points, _storageConfiguration.Database, "ISP Snitch By StefmDE");
			}

			storeResult.WrittenPointDataCount = points.Count;
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