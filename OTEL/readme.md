# Starten containers
Vanuit de folder: ~\OTAP
Command: ` docker-compose up --build`
De optie --build is om de container opnieuw te builden zodat, anders komen wijzigingen niet door.

# Certificaat
Zie: [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-9.0)

Uitvoeren commando's:

```dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\heroapi.pfx -p secret```

```dotnet dev-certs https --trust```

Na aanmaken certificaat heb ik het gekopieerd naar de https folder in de solution folder

# Testen
https://localhost:5002/Hero
