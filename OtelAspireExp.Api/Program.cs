
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace OtelAspireExp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.AddAspireExtensions();

        builder.Services.AddHttpLogging(x => {x. });


        builder.Services.AddMetrics();
        builder.Services.AddSingleton<OtelAspireExtMetrics>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHealthChecks("/health");

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseHttpLogging();

        app.MapControllers();

        app.Run();
    }
}
