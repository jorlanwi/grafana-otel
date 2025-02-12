# Overzicht
Zie ook: [Wiki](https://github.com/jorlanwi/grafana-otel/wiki)

Dit testproject is bedoeld om te spelen met Open Telemetry (OTEL) vanuit een .Net applicatie.

De opzet is een eenvoudige standaard WebApi en die voorzien van OTEL features. De .Net applicatie exporteert OTEL-data naar de applicatie `otel-collector`. De collector stuurt de data door naar Jaeger (traces) en Prometheus (metrics). Grafana wordt gebruikt om de metrics in Prometheus te visualiseren.

Er zijn readme's op verschillende niveaus om specifieker toelichting te geven.
