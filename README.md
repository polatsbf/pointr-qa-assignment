# Pointr QA Assignment

This repository covers:

- Question #1 (REST)
- Question #2 (UI)

## Stack

- ASP.NET Core Web API (.NET 8)
- xUnit
- `HttpClient` for API automation
- Playwright for UI automation
- Reqnroll with Gherkin syntax for BDD UI scenarios
- ExtentReports for UI test reporting
- GitHub Actions for CI

## Prerequisites

- .NET 8 SDK
- Internet access for NuGet restore and Playwright browser download

Playwright does not need to be preinstalled on the machine. The commands below restore the required tool and install the required browsers.

## Project structure

- `Pointr.Api`: REST API
- `tests/Pointr.Api.Tests`: automated API tests
- `tests/Pointr.Ui.Tests`: Playwright UI test project with Reqnroll/Gherkin
- `tests/Pointr.Ui.Tests/Features`: Gherkin feature files
- `tests/Pointr.Ui.Tests/Steps`: step definitions
- `tests/Pointr.Ui.Tests/Pages`: page object classes
- `tests/Pointr.Ui.Tests/Support`: shared utilities and scenario context
- `docs/test-cases.md`: manual test case list
- `.github/workflows/dotnet-ci.yml`: CI pipeline

## Implemented endpoints

### Site API

- `POST /api/sites`
- `GET /api/sites/{siteId}`
- `DELETE /api/sites/{siteId}`

### Building API

- `POST /api/sites/{siteId}/buildings`
- `GET /api/sites/{siteId}/buildings/{buildingId}`
- `DELETE /api/sites/{siteId}/buildings/{buildingId}`

### Levels API

- `POST /api/buildings/{buildingId}/levels`
- `POST /api/buildings/{buildingId}/levels/bulk`

## Notes

- The API uses an in-memory repository instead of a real database.
- This keeps the implementation focused on API design, contract validation, and test automation.
- The automated test cases are implemented with `xUnit` and `HttpClient`.

## Sample payloads

### Create site

```json
{
  "name": "Antalya Hospital Campus",
  "campusCode": "AHC-001",
  "address": "100 Antalya Health Street"
}
```

### Create building

```json
{
  "name": "Main Tower",
  "externalReference": "BLD-001"
}
```

### Import multiple levels

```json
{
  "levels": [
    {
      "name": "Ground Floor",
      "ordinal": 0
    },
    {
      "name": "First Floor",
      "ordinal": 1
    }
  ]
}
```

## Run API locally

```bash
dotnet restore
dotnet build
dotnet run --project Pointr.Api
```

## Run API tests

```bash
dotnet test tests/Pointr.Api.Tests/Pointr.Api.Tests.csproj
```

## Run UI tests

Install the Playwright tool and the required browsers, then run the UI project:

```bash
dotnet tool restore
dotnet build
dotnet tool run playwright install chromium firefox
dotnet test tests/Pointr.Ui.Tests/Pointr.Ui.Tests.csproj
```

The Gherkin scenarios are defined in:

- `tests/Pointr.Ui.Tests/Features/BlogAnalysis.feature`

The UI project covers two separate requirements:

- verify that all blog articles are loaded
- analyze the latest 3 articles, calculate the top 5 repeated words, and save the result to a `.txt` file

Both UI requirements run in:

- `chromium`
- `firefox`

The UI test writes the top 5 repeated words from the latest 3 blog articles into:

- `tests/Pointr.Ui.Tests/Output/latest-3-articles-top-5-words.txt`

The UI test run also generates an ExtentReports HTML report:

- `tests/Pointr.Ui.Tests/Report/report.html`

If a UI scenario fails, a full-page screenshot is saved under:

- `tests/Pointr.Ui.Tests/Report/screenshots`

## Run everything

After Playwright browsers are installed, you can also run the full suite with:

```bash
dotnet test
```

## CI

The GitHub Actions pipeline restores dependencies, builds the solution, installs Playwright browsers, and runs both the API and UI test suites on every push and pull request.

UI test artifacts are uploaded from CI when available:

- ExtentReports HTML report
- failure screenshots
- latest 3 articles top 5 words `.txt` files
