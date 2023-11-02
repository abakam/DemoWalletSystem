# BetWalletSystem

## A Demo App for a Wallet System

- Deposits are asynchronous, while withdrawals are synchronous.
- The background task handles asynchronous deposit transactions

### Requirements

This demo app targets .NET 6 and uses MS SQL for database.

In order to build and run the demo app, you need to install the following:

- .NET 6 SDK
- .NET CLI
- Visual Studio 2022 or its equivalent.

### Steps to run the demo app:

1. Clone the repository
2. Open the project using Visual Studio 2022
3. Replace the Default connection in appsettings.json with your MS SQL db connection string
4. Run **dotnet restore** command from the project directory to restore packages that the project references
5. Configure the launch profile in launchsettings.json
6. Build and run the demo app from Visual Studio

The app would run on the local host ports configured in launchsettings.json depending on the profile configured.
