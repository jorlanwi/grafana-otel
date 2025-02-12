# Starten containers
Vanuit de folder: ~\OTAP
Command: ` docker-compose up --build`
De optie --build is om de container opnieuw te builden, anders komen wijzigingen niet door.

## Docker-compose.yml
De volgende containers worden gestart:
- otel_jaeger_1: app voor weergeven traces
- otel_otel-collector_1: app om OTEL data te verzamalen en verspreiden
- otel_prometheus_1: app om metrics te doorzoeken
- otel_app_1: de test app
- otel_grafana_1: dashboard voor metrics

### Network
Definitie van het netwerk. Alle containers moeten aan hetzelfde netwerk worden gekoppeld, anders kunnen de container elkaar niet vinden. Containers verwijzen naar het netwerk via de 'definitie', in dit geval `metrics` en niet de naam (`hero-network`)

### prometheus
Er wordt een nieuwe container gemaakt op basis van de docker file in `./scripts/prometheus`. Deze is gebaseerd een standaard Prometheus image met daaraan toegevoegd onze configuratie.

Prometheus kan in de browser worden benaderd via http://localhost:9090

### grafana
Gelijk aan Prometheus, maar veel complexere configuratie.
Grafana kan in de browser worden benaderd via http://localhost:3001 (mijn port 3000 was al bezet)

### grafana
Standaard Jaeger image.

In de port-mappings is eigenlijk alleen 16686 nodig. De ander porten worden niet vanaf de host machine gebruikt.

Jaeger kan in de browser worden benaderd via http://localhost:16686

### otel-collector
Standaard image. Het image wordt gestart met de commandline parameter `--config=/etc/otel-collector-config.yaml`
Via het volume wordt het bestand `etc/otel-collector-config.yaml` gekoppeld aan het bestand `./scripts/otel-collector/otel-collector-config.yaml`

Omdat de otel-collector niet wordt benaderd vanaf het host systeem, kunnen alle port mappings worden verwijderd.

> In de configuratie van de otel-collector luistert het gRPC endpoint op 0.0.0.0:4317. Hierin is 0.0.0.0 belangrijk zodat otel luister op alle toegewezen ip-adressen van de container.

### app
Bouwt een nieuw Docker image o.b.v. de docker file in the app folder.
Via het volume wordt is het https cerificaat beschikbaar in de container.

# Certificaat
Zie: [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-9.0)

Uitvoeren commando's:
```
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p secret
dotnet dev-certs https --trust
```

Na aanmaken certificaat heb ik het gekopieerd naar de https folder in de solution folder

# Testen
HTTP GET https://localhost:5002/Hero

# Bronnen
- [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-9.0)