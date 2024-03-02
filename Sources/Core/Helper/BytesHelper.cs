namespace Core.Helper;

public static class BytesHelper
{
	public static readonly List<string> Units = ["byte/s", "kbyte/s", "mbyte/s", "gbyte/s", "tbyte/s"];
	
	public static string ToReadableSpeedUnit(this long bytes)
	{
		float value = bytes;
		var index = 0;
		while (value > 1024 && index < Units.Count)
		{
			value /= 1024;
			index++;
		}

		return $"{Math.Round(value, 2)} {Units[index]}";
	}
}