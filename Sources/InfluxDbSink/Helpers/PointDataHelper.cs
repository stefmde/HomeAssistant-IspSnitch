using System;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace InfluxDbSink.Helpers;

public class PointDataHelper
{
	private readonly DateTime _created;
	private readonly string _source;
	private readonly string _instanceLabel;

	public PointDataHelper(string instanceLabel, string source, DateTime created)
	{
		_created = created;
		_source = source;
		_instanceLabel = instanceLabel;
	}
	
	public PointData CreatePoint(string measurement, string value)
	{
		return CreateGenericPoint(measurement)
			.Field("value", value);
	}
	
	public PointData CreatePoint(string measurement, int value)
	{
		return CreateGenericPoint(measurement)
			.Field("value", value);
	}
	
	public PointData CreatePoint(string measurement, bool value)
	{
		return CreateGenericPoint(measurement)
			.Field("value", value);
	}
	
	public PointData CreatePoint(string measurement, float value)
	{
		return CreateGenericPoint(measurement)
			.Field("value", value);
	}
	
	private PointData CreateGenericPoint(string measurement)
	{
		return PointData.Measurement($"{_instanceLabel}-{measurement}")
			.Tag("source", _source)
			.Timestamp(_created, WritePrecision.S);
	}
}