using System.Net.NetworkInformation;
using Core.Models.Configuration.Pingtest;
using PingTest.Models;

namespace PingTest;

public class PingTest
{
	private readonly PingTestConfiguration _pingTestConfiguration;

	public PingTest(PingTestConfiguration pingTestConfiguration)
	{
		_pingTestConfiguration = pingTestConfiguration;
	}

	public async Task<PingTestResult> Test()
	{
		var ping = new Ping();
		var pingNativeResponse = await ping.SendPingAsync(_pingTestConfiguration.Address, TimeSpan.FromSeconds(_pingTestConfiguration.TimeoutMs));
		var pingTestResult = new PingTestResult
		{
			Address = _pingTestConfiguration.Address,
			IsSuccess = pingNativeResponse.Status == IPStatus.Success,
			Status = Enum.GetName(typeof(IPStatus), pingNativeResponse.Status),
			RoundtripTime = pingNativeResponse.RoundtripTime,
			ResponseIp = pingNativeResponse.Address.ToString()
		};

		return pingTestResult;
	}
}