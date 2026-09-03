# FunderMaps (legacy C# / .NET) — ARCHIVED

**This codebase is retired.** Nothing from it runs anywhere since 2026-08-29, when the
last component (the `/api/v3` Webservice) was switched off. It is kept read-only as a
reference for legacy behaviour.

The platform lives on in TypeScript/Bun services:

| what | repository | endpoint |
|---|---|---|
| platform API (CRUD, auth/OIDC, management) | [FunderMapsApi](https://github.com/Laixer/FunderMapsApi) | api.fundermaps.com |
| billable product API (`/v4/product/*`) | [FunderMapsWebservice](https://github.com/Laixer/FunderMapsWebservice) | ws.fundermaps.com |
| schema owner (`schema.sql`), background jobs, BAG loader, tile server | [FunderMapsWorker](https://github.com/Laixer/FunderMapsWorker) | tiles.fundermaps.com |

**Issues:** this tracker was the product tracker until the archive. The state at
archive time and a transfer script are in [`docs/tracker/`](docs/tracker/); new
issues go to the repository that owns the code in question.

## What was here

.NET 8 solution: `FunderMaps.WebApi` (platform API), `FunderMaps.Webservice`
(product API), `FunderMaps.Core` / `FunderMaps.Data` (domain + Dapper data layer),
`FunderMaps.AspNetCore`, plus `contrib/` deployment bits. The database schema it
targeted is the same PostgreSQL/PostGIS database the new services use; the
authoritative dump is `FunderMapsWorker/schema.sql`, **not** anything in this repo.

Legacy build/run instructions (kept for the record): .NET 8 SDK + Docker,
`./scripts/setupdb.sh` then `./scripts/loaddb.sh`, `dotnet run --project src/FunderMaps.WebApi`.
