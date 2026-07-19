# Blazesoft Slot Machine API
## Overview

This project is a .NET 8 Web API that simulates a slot machine game using MongoDB as the database.

The API allows players to:
- Create a player
- Update their balance
- Spin the slot machine
- Change the slot machine dimensions without restarting the application

The slot machine generates a random matrix of digits (0–9), evaluates configured win lines, calculates winnings, and updates the player's balance.

---

## Technologies

- .NET 8 Web API
- MongoDB Atlas
- MongoDB.Driver
- Swagger (OpenAPI)
- xUnit

---

## Project Structure

```
IrinCibiBlazesoftChallenge
│
├── Controllers
├── Models
├── Services
├── Configuration
├── Program.cs
├── appsettings.json
│
└── IrinCibiBlazesoftChallenge.Tests
```

---

## Setup

### 1. Clone the repository

```
git clone <repository-url>
```

---

### 2. Open the solution

Open the solution in Visual Studio.

---

### 3. Configure MongoDB

Update the connection string inside:

```
appsettings.json
```

Example:

```json
"MongoDbSettings": {
    "ConnectionString": "your-mongodb-connection-string",
    "DatabaseName": "Blazesoft"
}
```

---

### 4. Restore NuGet packages

Visual Studio will automatically restore packages.

Or run

```
dotnet restore
```

---

### 5. Run the application

```
dotnet run
```

Swagger will open automatically.
After the application starts, Swagger UI will be available at:

https://localhost:<port>/swagger

Use Swagger to test all API endpoints.

---

## API Endpoints

### Create Player

```
POST /api/slot/player
```

Example request

```json
{
    "username": "Aaron",
    "balance": 500
}
```

---

### Spin

```
POST /api/slot/spin
```

Example request

```json
{
    "playerId": "<player-id>",
    "betAmount": 10
}
```

Returns

- Slot machine matrix
- Win amount
- Updated balance
- Winning line details

---

### Update Balance

```
POST /api/slot/balance
```

Example

```json
{
    "playerId": "<player-id>",
    "amount": 100
}
```

---

### Update Slot Machine Configuration

```
POST /api/slot/config
```

Example

```json
{
    "width": 5,
    "height": 3
}
```

The updated configuration is stored in MongoDB and is used immediately without restarting the application.

---

## Slot Machine Rules

- Symbols range from 0–9.
- Winning combinations start from the first column.
- At least three consecutive matching symbols are required.
- Win amount = Bet × Sum of all winning line multipliers.
- Supported win lines:
  - Horizontal rows
  - Zig-zag diagonal paths

---

## Unit Tests

Unit tests are implemented using **xUnit**.

To run the tests:

```
dotnet test
```

The tests verify:

- Matrix generation
- Matrix dimensions
- Generated values
- Win calculation
- Win line generation
- Edge cases

---

## Notes

- Player balances are updated atomically using MongoDB to support simultaneous requests.
- Slot machine dimensions are configurable through the API without restarting the application.
- MongoDB Atlas is used as the backing database.
