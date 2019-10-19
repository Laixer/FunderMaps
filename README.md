# FunderMaps

[![Build Status](https://dev.azure.com/Laixer/FunderMaps/_apis/build/status/Laixer.FunderMaps?branchName=develop)](https://dev.azure.com/Laixer/FunderMaps/_build/latest?definitionId=1&branchName=develop)

More and more foundation issues occur and are actively investigated by local government. Gaining insight and acquiring more detailed data, **FunderMaps** enables municipalities and large property owners to register this data at the property level. Enrichment with data from Remote Monitoring or national data layers, various analyzes are performed that provide insight into the risk of foundation damage.

## Running the application

After cloning or downloading the application you should be able to run it with a persistent database. You will need to run its Entity Framework Core migrations before you will be able to run the app, and update the `ConfigureServices` method in `Startup.cs` (see below).

### Configuring the sample to use PostgreSQL

1. Ensure your connection strings in `appsettings.json` point to a PostgreSQL instance.

1. Open a command prompt in the Web folder and execute the following commands:

```
dotnet restore
dotnet ef database update
```

These commands will create for the database for the app's user credentials and identity data.

1. Run the application.

The first time you run the application, it will seed both databases with data such that you should see products in the store, and you should be able to log in using the admin@contoso.com account.

Note: If you need to create migrations, you can use this commands:

```
-- create migration (from Web folder CLI)
dotnet ef migrations add InitialModel -o Data/Migrations
```
