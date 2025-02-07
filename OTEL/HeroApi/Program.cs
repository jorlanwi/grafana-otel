using HeroApi.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HeroMetrics>();

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithMetrics(opts => opts
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HeroApi"))
        .AddMeter(builder.Configuration.GetValue<string>("HeroApiMeterName"))
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddMeter("Microsoft.AspNetCore.Http.Connections")
        .AddMeter("Microsoft.AspNetCore.Routing")
        .AddMeter("Microsoft.AspNetCore.Diagnostics")
        .AddMeter("Microsoft.AspNetCore.RateLimiting")
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        //.AddConsoleExporter()
        .AddOtlpExporter(opts =>
        {
            opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
        })
    )
    .WithTracing(opts => opts
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(opts =>
        {
            opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
        }));
        //.AddConsoleExporter()); 

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
