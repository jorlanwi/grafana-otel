using System.Diagnostics;

namespace HeroApi.Metrics;

public class HeroTelemetry
{
    private readonly IConfiguration _configuration;
    private readonly ActivitySource _activitySource;
    public readonly string _meterName;

    public HeroTelemetry(IConfiguration configuration)
    {
        _configuration = configuration;
        _activitySource = new(_configuration.GetValue<string>("ActivitySourceName"));
        _activitySource = new(_configuration.GetValue<string>("MeterName"));
    }

    public ActivitySource ActivitySource => _activitySource;
    public string MeterName => _meterName;
}
