# ShopPay MCP Server - Phase 1

This document explains how the MCP (Model Context Protocol) Server was integrated into the existing ShopPay Clean Architecture solution and how to verify it using the MCP Inspector.

---

# Prerequisites

- .NET 8 SDK
- Visual Studio 2022
- Node.js (Required for MCP Inspector)
- PostgreSQL
- Existing ShopPay Solution

---

# Running the MCP Inspector

Start the MCP Inspector using:

```bash
npx @modelcontextprotocol/inspector
```

After a few seconds you'll see something similar to:

```
MCP Inspector running...

Web UI:
http://localhost:6274
```

Open the URL in your browser.

---

# Connecting the Inspector

Configure the Inspector as follows:

| Property | Value |
|----------|-------|
| Transport Type | Streamable HTTP |
| URL | http://localhost:5000 |
| Connection Type | Via Proxy |

Click **Connect**.

If everything is configured correctly, the Inspector will display:

- Connected
- Server Name
- Available Tools

---

# Existing Solution Structure

Before adding MCP, the solution looked like this:

```
ShopPay
│
├── ShopPay.API
├── ShopPay.Application
├── ShopPay.Domain
└── ShopPay.Infrastructure
```

---

# Creating the MCP Server

A new project was added.

```
ShopPay.McpServer
```

Project Type:

```
Console App (.NET 8)
```

Updated Solution:

```
ShopPay
│
├── ShopPay.API
├── ShopPay.Application
├── ShopPay.Domain
├── ShopPay.Infrastructure
└── ShopPay.McpServer
```

The MCP Server acts as another entry point into the application.

---

# Why a Separate MCP Server?

Instead of exposing business logic through REST Controllers only, MCP exposes it as AI tools.

```
Angular
    │
REST API
    │
Controllers
    │
Application Layer
```

and

```
Azure OpenAI / ChatGPT
            │
        MCP Server
            │
Application Layer
```

Both use the same business logic.

No duplication occurs.

---

# Project References

The following project references were added to the MCP Server.

```
✔ ShopPay.Application
✔ ShopPay.Domain
✔ ShopPay.Infrastructure
```

The API project was **not** referenced.

Reason:

Both API and MCP Server are entry points.

```
                Application
                     ▲
                     │
        ┌────────────┴────────────┐
        │                         │
 ShopPay.API              ShopPay.McpServer
```

---

# NuGet Package

Installed package:

```
ModelContextProtocol.AspNetCore
```

This package provides:

- MCP Server
- Tool Registration
- Streamable HTTP Transport
- Dependency Injection Integration

---

# Configuration Files

Copied:

```
appsettings.json

appsettings.Development.json
```

These provide:

- Connection String
- Database Configuration
- Application Settings

The MCP Server uses the same database as the REST API.

---

# Dependency Injection

The existing DI registrations were reused.

```csharp
builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);
```

No services were duplicated.

---

# Program.cs

```csharp
using ModelContextProtocol.AspNetCore;
using ShopPay.Application;
using ShopPay.Infrastructure;
using ShopPay.McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<ProductTools>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapMcp();

app.Run();
```

---

# Creating the Tool

A folder was created.

```
Tools
```

Inside it:

```
ProductTools.cs
```

---

# Product Tool

```csharp
using ModelContextProtocol.Server;

[McpServerToolType]
public class ProductTools
{
    private readonly IProductService _productService;

    public ProductTools(IProductService productService)
    {
        _productService = productService;
    }

    [McpServerTool]
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        return await _productService.GetAllProductsAsync();
    }
}
```

Notice:

REST API

```csharp
[HttpGet]
public async Task<IActionResult> GetProducts()
```

became

```csharp
[McpServerTool]
public async Task<IEnumerable<ProductDto>> GetProducts()
```

No ControllerBase.

No IActionResult.

No HTTP Route.

Only business logic.

---

# Running the MCP Server

Set

```
ShopPay.McpServer
```

as the Startup Project.

Run the application.

Expected output:

```
Now listening on:
http://localhost:5000

Application started.
```

This confirms:

- MCP Server started successfully
- Dependency Injection resolved correctly
- Database configuration is valid

---

# Verifying with MCP Inspector

After connecting:

Click

```
List Tools
```

The Inspector should discover:

```
get_products
```

Select the tool.

Click

```
Run Tool
```

Expected Result:

```json
[
  {
    "productId": 1,
    "name": "Dell Laptop",
    "price": 10000
  },
  {
    "productId": 3,
    "name": "Samsung TV",
    "price": 12000
  }
]
```

The data comes directly from the PostgreSQL database through the existing Application Layer.

---

# Complete Flow

```
MCP Inspector
        │
        ▼
Streamable HTTP
        │
        ▼
ShopPay.McpServer
        │
        ▼
ProductTools
        │
        ▼
IProductService
        │
        ▼
ProductRepository
        │
        ▼
EF Core
        │
        ▼
PostgreSQL
```

---

# Comparison with REST API

REST API

```
Angular
      │
HTTP GET
      │
Controller
      │
Service
      │
Repository
      │
Database
```

MCP

```
AI Client
      │
Tool Call
      │
ProductTools
      │
Service
      │
Repository
      │
Database
```

The Application Layer remains exactly the same.

Only the entry point changes.

---

# Benefits of Option 2

- Reuses existing business logic
- No duplicate services
- No duplicate repositories
- Better performance (no internal HTTP calls)
- Follows Clean Architecture
- Easy to maintain
- Enterprise-friendly architecture

---

# Current Status

Completed:

- Created MCP Server project
- Added project references
- Installed MCP SDK
- Configured Streamable HTTP transport
- Reused existing Dependency Injection
- Created first MCP Tool
- Successfully connected using MCP Inspector
- Successfully executed the `get_products` tool
- Retrieved data from PostgreSQL through the existing Application Layer

---

# Next Phase

The next step is to integrate an AI client (such as Azure OpenAI or ChatGPT) with the MCP Server.

The flow will become:

```
User
   │
   ▼
Azure OpenAI / ChatGPT
   │
Automatically selects Tool
   │
   ▼
ShopPay.McpServer
   │
   ▼
Application Layer
   │
   ▼
Database
```

At that stage, the LLM will automatically decide when to invoke tools like `get_products` and then generate a natural language response for the user.
