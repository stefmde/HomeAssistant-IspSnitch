using System.Diagnostics;
using System.Text;
using Core.Models.Configuration.Speedtest;
using Newtonsoft.Json;
using Speedtest.Models;
using Speedtest.Models.Ookla;

namespace Speedtest;

public class SpeedTest
{
	private readonly SpeedTestConfiguration _configuration;

	public SpeedTest(SpeedTestConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<SpeedTestResult> Test()
	{
		var result = new SpeedTestResult();
		var argumentsBuilder = new StringBuilder();
		argumentsBuilder.Append("--progress=no ");
		argumentsBuilder.Append("--format=json");
		argumentsBuilder.Append(_configuration.Debug ? "-pretty " : " ");
		argumentsBuilder.Append(_configuration.ForceServerById > 0 ? $"–server-id={_configuration.ForceServerById} " : string.Empty);
		argumentsBuilder.Append(string.IsNullOrWhiteSpace(_configuration.ForceServerByHostName) ? string.Empty : $"–host={_configuration.ForceServerByHostName} ");
		argumentsBuilder.Append(string.IsNullOrWhiteSpace(_configuration.ForceInterfaceByName) ? string.Empty : $"–interface={_configuration.ForceInterfaceByName} ");
		
		var startInfo = new ProcessStartInfo
		{
			FileName = "speedtest",
			Arguments = argumentsBuilder.ToString(),
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		if (_configuration.Debug)
		{
			result.DebugAppend($"Executed: '{startInfo.FileName}' with arguments: '{startInfo.Arguments}'");
		}
		
		try
		{
			var proc = Process.Start(startInfo);
			await proc.WaitForExitAsync();
			var resultJson = await proc.StandardOutput.ReadToEndAsync();
			
			if (_configuration.Debug)
			{
				result.DebugAppend($"Result Json: {resultJson}");
			}
			
			result.Error = await proc.StandardError.ReadToEndAsync();
			result.IsSuccess = string.IsNullOrWhiteSpace(result.Error) && !resultJson.Contains("error",StringComparison.InvariantCultureIgnoreCase);
			
			if (result.IsSuccess)
			{
				result.OoklaResult = JsonConvert.DeserializeObject<OoklaResult>(resultJson);

				if (_configuration.Debug)
				{
					result.DebugAppend("Readable:");
					result.DebugAppend(result.OoklaResult.ToString());
				}
			}
		}
		catch (Exception ex)
		{
			result.Error += ex.Message;
		}

		return result;
	}
}