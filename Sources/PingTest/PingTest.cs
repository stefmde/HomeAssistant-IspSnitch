using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Core.Models.Configuration.Tests.PingTest;
using PingTest.Models;

namespace PingTest;

public class PingTest
{
	private readonly PingTestConfiguration _pingTestBaseConfiguration;

	public PingTest(PingTestConfiguration pingTestBaseConfiguration)
	{
		_pingTestBaseConfiguration = pingTestBaseConfiguration;
	}

	public async Task<PingTestResult> Test()
	{
		var ping = new Ping();
		var pingNativeResponse = await ping.SendPingAsync(_pingTestBaseConfiguration.Address, TimeSpan.FromSeconds(_pingTestBaseConfiguration.TimeoutMs));
		var pingTestResult = new PingTestResult
		{
			Address = _pingTestBaseConfiguration.Address,
			IsSuccess = pingNativeResponse.Status == IPStatus.Success,
			Status = Enum.GetName(typeof(IPStatus), pingNativeResponse.Status),
			RoundtripTime = pingNativeResponse.RoundtripTime,
			ResponseIp = pingNativeResponse.Address.ToString()
		};

		return pingTestResult;
	}
}