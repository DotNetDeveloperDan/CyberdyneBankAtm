# 💳 CyberdyneBankAtm

**Cyberdyne: Where your funds become self-aware.**

A modern, web-based ATM simulator built with Blazor WebAssembly (WASM) and ASP.NET Core (.NET 8), using Clean Architecture principles. Manage Checking and Savings accounts: Deposit, Withdraw, Transfer — with real-time balances and transaction history.

## 🚀 Features

- .NET 9 Backend API: Minimal APIs, CQRS, and Clean Architecture structure.
- SQLite Database: Simple, file-based persistence.
- Transaction Types: Deposit, Withdraw, Transfer (between user’s own accounts).
- Atomic Balance Updates: Ensures account and transaction integrity.
- Live Transaction History: View by account.
- Global Notification System: Success & error popups without 3rd party libraries.
- Modern Coding Practices: CQRS, DI, async/await, solution folders, robust validation.

## 🗂️ Project Structure

```
/CyberdyneBankAtm
  /CyberdyneBankAtm.Api         # ASP.NET Core Minimal API (backend)
  /CyberdyneBankAtm.Client      # Blazor WebAssembly UI (frontend)
  /CyberdyneBankAtm.Application # CQRS, Handlers, Validators, Use Cases
  /CyberdyneBankAtm.Domain      # Entities, Aggregates, Enums
  /CyberdyneBankAtm.Infrastructure # EF Core, SQLite, DbContext, Migrations
  /CyberdyneBankAtm.SharedKernel   # Common abstractions (optional)
```

## ⚡️ Getting Started

1. **Prerequisites**
   - .NET 9 SDK
   - Visual Studio 2022+ or VS Code

2. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/CyberdyneBankAtm.git
   cd CyberdyneBankAtm
   ```

3. **Setup the Database**
   No manual step needed! On first run, the backend creates `atm.db` (SQLite) in the API’s output folder and applies any EF Core migrations.

4. **Run the Backend API**
   ```bash
   cd CyberdyneBankAtm.Api
   dotnet run
   ```
   The API will start on `https://localhost:8081` (default).

5. **Run the Blazor Client**
   Open a new terminal:
   ```bash
   cd CyberdyneBankAtm.Client
   dotnet run
   ```
   The client starts on `https://localhost:7282` (default).

## 🌐 Usage

Open `https://localhost:7282` in your browser.

Use the ATM interface to view accounts, deposit, withdraw, or transfer funds.

The ATM interface is in a separate repo and can be found here: [https://github.com/DotNetDeveloperDan/CyberdyneBankAtmClient](https://github.com/DotNetDeveloperDan/CyberdyneBankAtmClient)

All operations update balances and show transaction history in real time.

## 🔐 CORS Setup (Development)

The API project enables CORS for the Blazor client by default:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasmDev",
        policy => policy
            .WithOrigins("https://localhost:7282")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
```

## 👨‍💻 Tech Stack

- **Backend:** ASP.NET Core Minimal API (.NET 9)
- **Database:** SQLite via EF Core
- **Architecture:** Clean Architecture, CQRS, Dependency Injection
- **Notifications:** Built-in global alert system (no 3rd party)
- **Validation:** FluentValidation

## 🏗️ Extending

- Add new transaction types by updating `TransactionType` enum and handlers.
- Replace SQLite with SQL Server or PostgreSQL by swapping the provider in Infrastructure.
- Add authentication for multi-user support (future enhancement).

## 🙋 FAQ

**Q:** How are transactions kept atomic?  
**A:** Transfers use EF Core transactions to ensure both account balances and transaction history update together.

**Q:** Can I use a different front-end?  
**A:** Yes. The API is RESTful; any modern UI or mobile app can consume it.

## 📄 License

MIT.

CyberdyneBankAtm is for educational/demo use—no real money, please!

*CyberdyneBankAtm: Where your funds become self-aware.*
