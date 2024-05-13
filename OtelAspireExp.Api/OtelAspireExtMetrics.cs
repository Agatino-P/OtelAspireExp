using System.Diagnostics.Metrics;
using System.Runtime.InteropServices.ObjectiveC;

namespace OtelAspireExp.Api;

public class OtelAspireExtMetrics
{
    public const string MeterName = "OtelAspireExp.Api";
    private readonly Counter<long> _otelAspireExpRequestCounter;
    private readonly Histogram<double> _otelAspireExpRequestDuration;

    public OtelAspireExtMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _otelAspireExpRequestCounter = meter.CreateCounter<long>(
            "otelaspireexp.api.otelaspireexp_requests.count.");
        _otelAspireExpRequestDuration = meter.CreateHistogram<double>(
            "otelaspireexp.api.otelaspireexp_requests.duration.", "ms");

    }

    public void IncreaseOtelAspireExpRequestCount()
    {
        _otelAspireExpRequestCounter.Add(1);
    }
    public TrackedRequestDuration MeasureRequestDuration()
    {
        return new TrackedRequestDuration(_otelAspireExpRequestDuration);
    }

}