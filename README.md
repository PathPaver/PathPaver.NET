# PathPaver Backend

Clean Architecture ASP.NET Core project using .NET 8.0

## Requirements

- .NET 8.0

## Setup

### Restore Packages

Same level of .sln

```sh
docker restore
```

### Add Cluster informations in Users Secrets

You need to go to `src/PathPaver.Web/` and run the following commands with the correct values.

```sh
dotnet user-secrets set "MongoCluster:ConnectionURI" "$THE_MONGODB_URI"
dotnet user-secrets set "MongoCluster:DatabaseName" "$THE_DATABASE_NAME"
dotnet user-secrets set "FrontendUrl" "http://localhost:3000"
dotnet user-secrets set "Security:PrivateKey" "$PRIVATE_KEY"
```

### To create XML report with Coverlet for all Tests projects in solution

```shell
# Install ReportGenerator tool at a global level
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate xml coverage files
dotnet test --collect:"XPlat Code Coverage" "PathPaver.sln" --results-directory:"coverage/"

# Generate HTML Report with ReportGenerator
dotnet reportgenerator "-reports:coverage/*/coverage.cobertura.xml" "-targetdir:coverage/report/coveragereport" -reporttypes:Html
```

## Domain Layer

the project that contains the domain layer, including the entities, value objects, and domain services

## Application Layer

the project that contains the application layer and implements the application services, DTOs (data transfer objects), and mappers. It should reference the Domain project.

## Infrastructure Layer

The project contains the infrastructure layer, including the implementation of data access, logging, email, and other communication mechanisms. It should reference the Application project.

## Presentation Layer

The main project contains the presentation layer and implements the ASP.NET Core web API. It should reference the Application and Infrastructure projects.

## Stress Tests

This project uses a tool called artillery to run stress tests on our website.

Before running this test, you must create a folder called "test-data", then add a json file called "login.json" inside the folder. This will be used by the stress test script to log into the backend and generate a valid token.

Next you'll need to download a tool called "jq". It allows json parsing through the command terminal. This is required for the run-test-load-script.sh file, to extract the token and store it in a csv file.

Finally, to run the tests, you just need to run the file run-test-load-script.sh in a terminal, which will automatically do everything required for the artillery tests.
