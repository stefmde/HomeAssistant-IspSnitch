using System.Diagnostics;
using System.Text;
using Core.Models.Configuration.Tests.SpeedTest;
using Newtonsoft.Json;
using SpeedTest.Models;
using SpeedTest.Models.Ookla;

namespace SpeedTest;

public class SpeedTest
{
	private readonly SpeedTestConfiguration _baseConfiguration;

	public SpeedTest(SpeedTestConfiguration baseConfiguration)
	{
		_baseConfiguration = baseConfiguration;
	}

	public async Task<SpeedTestResult> Test()
	{
		var result = new SpeedTestResult();
		var argumentsBuilder = new StringBuilder();
		argumentsBuilder.Append("--progress=no ");
		argumentsBuilder.Append("--format=json");
		argumentsBuilder.Append(_baseConfiguration.Debug ? "-pretty " : " ");
		argumentsBuilder.Append(_baseConfiguration.ForceServerById > 0 ? $"–server-id={_baseConfiguration.ForceServerById} " : string.Empty);
		argumentsBuilder.Append(string.IsNullOrWhiteSpace(_baseConfiguration.ForceServerByHostName) ? string.Empty : $"–host={_baseConfiguration.ForceServerByHostName} ");
		argumentsBuilder.Append(string.IsNullOrWhiteSpace(_baseConfiguration.ForceInterfaceByName) ? string.Empty : $"–interface={_baseConfiguration.ForceInterfaceByName} ");
		
		var startInfo = new ProcessStartInfo
		{
			FileName = "speedtest",
			Arguments = argumentsBuilder.ToString(),
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		if (_baseConfiguration.Debug)
		{
			result.DebugAppend($"Executed: '{startInfo.FileName}' with arguments: '{startInfo.Arguments}'");
		}
		
		try
		{
			var proc = Process.Start(startInfo);
			await proc.WaitForExitAsync();
			var resultJson = await proc.StandardOutput.ReadToEndAsync();
			
			if (_baseConfiguration.Debug)
			{
				result.DebugAppend($"Result Json: {resultJson}");
			}
			
			result.Error = await proc.StandardError.ReadToEndAsync();
			result.IsSuccess = string.IsNullOrWhiteSpace(result.Error) && !resultJson.Contains("error",StringComparison.InvariantCultureIgnoreCase);
			
			if (result.IsSuccess)
			{
				result.OoklaResult = JsonConvert.DeserializeObject<OoklaResult>(resultJson);

				if (_baseConfiguration.Debug)
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