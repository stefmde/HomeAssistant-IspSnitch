using Core.Models.Configuration.Sinks.HomeAssistantSink;
using HomeAssistantSink.Helpers;
using HomeAssistantSink.Models;
using Newtonsoft.Json;
using PingTest.Models;

namespace HomeAssistantSink;

public class HomeAssistantSink
{
	private readonly string _baseUrl;
	private readonly bool _tokenIsSet;
	private readonly HttpClient _httpClient;
	private readonly HomeAssistantSinkConfiguration _configuration;

	public HomeAssistantSink(HomeAssistantSinkConfiguration configuration)
	{
		_configuration = configuration;
		var token = Environment.GetEnvironmentVariable("SUPERVISOR_TOKEN");
		_tokenIsSet = !string.IsNullOrWhiteSpace(token);
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.Add("Authorization", token);
		_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
		_baseUrl = $"http://supervisor/core/api/states/";
	}

	public async Task<HomeAssistantSinkResult> StorePingTestData(PingTestResult pingTestResult)
	{
		if (!_tokenIsSet)
		{
			return new HomeAssistantSinkResult { ErrorType = HomeAssistantSinkResultErrorType.ErrorTokenNotSet };
		}

		if (!_configuration.Enabled)
		{
			return new HomeAssistantSinkResult { ErrorType = HomeAssistantSinkResultErrorType.SkippedDueToGlobalDisabled };
		}

		var recordHelper = new PingTestRecordDataHelper(pingTestResult, _configuration.SinkAmountType);
		var records = recordHelper.GetRecords();
		var storeResult = await StoreGenericRecordData(records);
		return storeResult;
	}

	private async Task<HomeAssistantSinkResult> StoreGenericRecordData(List<EntityDataRecord> records)
	{
		var result = new HomeAssistantSinkResult();
		
		foreach (var record in records)
		{
			try
			{
				if(_configuration.Debug) Console.WriteLine($"Writing state '{record.EntityData.State}' to Home Assistant for EntityId '{record.EntityId}'");
				var json = JsonConvert.SerializeObject(record.EntityData);
				if(_configuration.Debug) Console.WriteLine(json);
				var httpData = new StringContent(json);
				var response = await _httpClient.PostAsync($"{_baseUrl}{record.EntityId}", httpData);
				var responseSuccess = response.IsSuccessStatusCode;
			
				if (responseSuccess)
				{
					continue;
				}
			
				if (_configuration.Debug & !responseSuccess)
				{
					Console.WriteLine($" Unable to write to Home Assistant. ResponseStatusCode: '{response.StatusCode}' and ResponseContent: '{ await response.Content.ReadAsStringAsync()}'");
				}
			
				result.FailedEntityIds.Add(record.EntityId);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unable to save EntityId '{record.EntityId}' with Exception: {ex}");
				result.FailedEntityIds.Add(record.EntityId);
			}
		}

		result.Success = !result.FailedEntityIds.Any();
		result.ErrorType = result.Success ? HomeAssistantSinkResultErrorType.None : HomeAssistantSinkResultErrorType.ErrorApi;
		return result;
	}
}