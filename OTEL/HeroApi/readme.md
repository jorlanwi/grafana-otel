# Stappen
- Standaard WebApi
- NuGet:
  - OpenTelemetry.Extensions.Hosting 
  - OpenTelemetry.Instrumentation.AspNetCore
  - OpenTelemetry.Exporter.Console
  - OpenTelemetry.Exporter.OpenTelemetryProtocol
  - OpenTelemetry.Instrumentation.* (bijv. HttpClient, Process, Runtime, SQl, EF etc)

Zie Program.cs voor configuratie van OTEL.
Voor debuggen kan `AddConsoleExporter()` worden gebruikt. 

# Custom Metrics
Zie folder Metrics.
Door Metrics toe te voegen kan hebben we meer inzicht in de applicatie

 
# Bronnen
- [Built-in metrics in .NET](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics)
- [Getting started with OpenTelemetry Metrics in .NET 8. Part 2: Instrumenting the BookStore API](https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-metrics-and-dotnet-part-2/)