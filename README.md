# Haystac
![Alt text](<images/HaySTAC_Logo_500x300.png>)

- [Haystac](#haystac)
  - [Overview](#overview)
  - [Technologies](#technologies)
- [Getting Started](#getting-started)
  - [Docker Compose](#docker-compose)

## Overview

`Haystac` is an ASP.NET Spatio-Temporal Asset Catalog ([STAC](https://github.com/radiantearth/stac-spec)) solution that offers both a web API and client CLI for interacting with your STAC entities.

The goal of this project is offer an open-source, secured, and configurable STAC solution that is easily extendable to any consumer's use case.

The project is heavily inspired by the [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/tree/net7.0) project template developed by Jason Taylor et al.

## Technologies
* [ASP.NET Core 7](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core 7](https://docs.microsoft.com/en-us/ef/core/)
* [Npgsql & NetTopologySuite](https://www.npgsql.org/efcore/mapping/nts.html?tabs=with-datasource)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)

# Getting Started

## Docker Compose

`Haystac` provides a `docker-compose` file that will configure & stand-up a test API container alongside a PostGIS container that consumers can use to validate usage as if deployed into a cloud environment.

In order to support serving over HTTPS - you'll need to enter the following commands:
```shell
dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\haystac.pfx"  -p haystac
dotnet dev-certs https --trust
```

In the preceding commands, you can change the names of the certificate and the password - but you'll have to update the `docker-compose` file here:

```yaml
ASPNETCORE_Kestrel__Certificates__Default__Password: haystac
ASPNETCORE_Kestrel__Certificates__Default__Path: /https/haystac.pfx
```

Now, execute the following
```shell
docker-compose up -d
```

This will then build the images, and server the API over:
- http://localhost:8000
- https://localhost:8001

And then to tear things down, execute:
```shell
docker-compose down
```

See: [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-7.0)