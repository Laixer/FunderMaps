# FunderMaps

![FunderMaps Ecosystem](https://github.com/Laixer/FunderMaps/workflows/FunderMaps%20Ecosystem/badge.svg)

Foundation issues occur and are actively investigated by local government. Gaining insight and acquiring more detailed data, **FunderMaps** enables municipalities and large property owners to register this data at the property level. Enrichment with data from Remote Monitoring or national data layers, various analyzes are performed that provide insight into the risk of foundation damage.

## Requirements

* .NET 5 Runtime and SDK
* Docker

## Running the application on localhost

After cloning or downloading the application you will be able to run the application on localhost. You will need to run the setup and load scripts to configure the local database.

1. Run from solution directory. This will setup the PostgreSQL instance and seed the database.

```
./scripts/setupdb.sh
./scripts/loaddb.sh
```

3. Ensure your connection strings in `appsettings.{ENV}.json` point to the local PostgreSQL instance. For example the *FunderMaps.WebApi* would have `Server=localhost;Database=fundermaps;User Id=fundermaps_webapp` where the user id corresponds to the application.

4. Enter the project directory in `src/{project}`.

5. Run the application with `ASPNETCORE_ENVIRONMENT={ENV} dotnet run`. The application should keep running in the foreground. The foreground logging shows the connection details.

When using VS Code the 'debug' section should list all the applications. Just run the application (or hit F5).

## Configuration

See the `contrib/etc/_appsettings.{ENV}.json` directory for configuration files for each environment. You can copy these configuration files to the project source directory.

## Using the application

See the user table below. All users use the same password: `fundermaps`.

| Name           | Email                 | Function      |
|----------------|-----------------------|---------------|
| Administrator  | admin@fundermaps.com  | Administrator |
|                | Javier40@yahoo.com    | Superuser     |
| kihn           | Freda@contoso.com     | Reviewer      |
| Patsy Brekke   | patsy@contoso.com     | Writer        |
| Lester Bednar  | lester@contoso.com    | Reader        |
|                | corene@contoso.com    | Reader        |
