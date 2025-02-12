using HeroApi.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);
builder.Logging.AddConsole();

builder.Services.AddSingleton<HeroMetrics>();
builder.Services.AddSingleton( _ => {
    return new HeroTelemetry(builder.Configuration);
});

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithMetrics(opts => opts
        .SetResourceBuilder(resourceBuilder)
        .AddMeter(builder.Configuration.GetValue<string>("HeroApiMeterName"))
        .AddAspNetCoreInstrumentation()
        //.AddMeter("Microsoft.AspNetCore.Hosting")
        //.AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        //.AddMeter("Microsoft.AspNetCore.Http.Connections")
        //.AddMeter("Microsoft.AspNetCore.Routing")
        //.AddMeter("Microsoft.AspNetCore.Diagnostics")
        //.AddMeter("Microsoft.AspNetCore.RateLimiting")
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        //.AddConsoleExporter()
        .AddOtlpExporter(opts =>
        {
            opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
        }))
    .WithTracing(opts => opts
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        //.AddConsoleExporter()
        .AddOtlpExporter(opts =>
        {
            opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
        }));

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(resourceBuilder)
        //.AddConsoleExporter()
        .AddOtlpExporter(opts =>
        {
            opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
        });
});

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
