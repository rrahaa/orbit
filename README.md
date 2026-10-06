# Smart Restaurant / Orbit

Blazor WebAssembly frontend + ASP.NET Core API + MySQL database for a school smart-restaurant project.

## What is implemented

- Restaurant ordering page at `/order` (also the default page)
- Tables loaded from `GET /api/tische`
- Products loaded from `GET /api/artikel`
- Product categories and search
- Cart with quantities, +/- controls and total
- Active employee selector from `GET /api/mitarbeiter`
- New orders sent to `POST /api/bestellungen`
- Table status refresh after ordering
- Friendly API/database error display
- Responsive tablet/desktop layout
- CORS configured for the included Web launch ports

## Database

The API connection string is in:

`SmartRestaurant.Api/appsettings.json`

Default:

`server=localhost;port=3307;database=orbit;user=orbit;password=orbit_pw`

The repository includes `docker-compose.yml`. From the solution directory you can start MySQL and phpMyAdmin with:

```bash
docker compose up -d
```

The SQL initialization file is in `Db/init/orbit.sql`.

- MySQL host port: `3307`
- phpMyAdmin: `http://localhost:8081`

## Visual Studio 2022

1. Install the **ASP.NET and web development** workload and a Visual Studio/.NET version that supports `net10.0`.
2. Open `SmartRestaurant.slnx`.
3. Configure multiple startup projects:
   - `SmartRestaurant.Api` -> Start
   - `SmartRestaurant.Web` -> Start
4. Start with the HTTPS profiles.
5. The Web app opens at `https://localhost:7102` and talks to the API at `https://localhost:7062`.

If you change the ports, update:

- API CORS origins in `SmartRestaurant.Api/Program.cs`
- API URL in `SmartRestaurant.Web/Program.cs`

## Frontend update: category filter and login preparation

- The manual refresh button on the billing page was removed.
- The order page now has a category select box including "Alle Kategorien".
- The employee select box was removed from the order page.
- Until login is implemented, orders use employee ID 1 as a temporary fallback because the existing API requires `MitarbeiterId`.
- No API/backend files were changed for this update.

## Tests

Automated tests live in `SmartRestaurant.Tests` and run on every push via GitHub Actions
(`.github/workflows/tests.yml`). Run them locally with `dotnet test SmartRestaurant.Tests`.
See [docs/TESTS.md](docs/TESTS.md) for the test report.
