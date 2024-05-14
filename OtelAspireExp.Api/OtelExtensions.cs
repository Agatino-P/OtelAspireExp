using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OtelAspireExp.Api;

public static class OtelExtensions
{
    public static IHostApplicationBuilder AddAspireExtensions(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();


        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.UseServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(x =>
        {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
            x.AddOtlpExporter();
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(x =>
            {
                x.AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "OtelAspireExp.Api");
            })
            .WithTracing(x =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    x.SetSampler<AlwaysOnSampler>();
                }

                x.AddAspNetCoreInstrumentation()
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }


    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

        builder.Services.AddOpenTelemetry().WithMetrics(x =>
        {
            x.AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http",
                "OtelAspireExp.Api"
                );
        });

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

}
