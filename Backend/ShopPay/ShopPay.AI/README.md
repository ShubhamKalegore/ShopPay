# Azure OpenAI + MCP Integration

This document explains the complete setup of integrating **Azure OpenAI** with an **MCP (Model Context Protocol) Server** in the ShopPay project.

---

# Architecture

```
Angular Client
      │
      ▼
ASP.NET Core Web API
      │
      ▼
AzureOpenAIService
      │
      ▼
Azure OpenAI (GPT-5 Mini)
      │
      │ Tool Calls
      ▼
MCP Client
      │
      ▼
MCP Server
      │
      ▼
Product / Order / Address Tools
```

The Azure OpenAI model determines whether a user's request requires a tool. If required, it calls the appropriate MCP tool, receives the result, and generates the final response.

---

# Prerequisites

- Azure Subscription
- Azure AI Foundry Project
- Azure OpenAI Model Deployment
- MCP Server running locally
- .NET 8 SDK

---

# NuGet Packages Installed

```bash
dotnet add package Azure.AI.OpenAI --version 2.1.0
```

```bash
dotnet add package Azure.Identity
```

```bash
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
```

```bash
dotnet add package ModelContextProtocol
```

---

# Azure Portal Configuration

## Step 1 Create Azure AI Foundry Resource

From Azure Portal create:

- Resource Group
- Azure AI Foundry Resource

Example

```
Resource Group
    ShopPay-RG

Azure AI Foundry
    ShopPay-AI
```

---

## Step 2 Open Azure AI Foundry

Open the Azure AI Foundry resource and launch the Foundry portal.

---

## Step 3 Create a Project

Inside Foundry

```
New Project
```

Example

```
ShopPayAI
```

---

## Step 4 Deploy GPT Model

Inside the project

```
Models + Endpoints
```

Select

```
Deploy Base Model
```

Choose

```
GPT-5 Mini
```

Provide

```
Deployment Name
```

Example

```
gpt-5-mini
```

After deployment note the following values

- Endpoint
- Deployment Name
- API Key

These values are required by the application.

---

# Store Secrets

Instead of storing secrets in appsettings.json, use User Secrets.

Initialize

```bash
dotnet user-secrets init
```

Store Endpoint

```bash
dotnet user-secrets set "AzureOpenAI:Endpoint" "<endpoint>"
```

Store API Key

```bash
dotnet user-secrets set "AzureOpenAI:ApiKey" "<api-key>"
```

Store Deployment Name

```bash
dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-5-mini"
```

---

# appsettings.json

```json
{
  "AzureOpenAI": {
    "Endpoint": "",
    "ApiKey": "",
    "DeploymentName": ""
  }
}
```

Values are loaded from User Secrets during development.

---

# Program.cs Changes

## Configure Azure OpenAI Settings

```csharp
builder.Services.Configure<AzureOpenAISettings>(
    builder.Configuration.GetSection("AzureOpenAI"));
```

---

## Register AzureOpenAIService

```csharp
builder.Services.AddScoped<AzureOpenAIService>();
```

---

## Create MCP Client

```csharp
var transportOptions = new HttpClientTransportOptions
{
    Endpoint = new Uri("http://localhost:5000")
};

var transport = new HttpClientTransport(transportOptions);

var client = await McpClient.CreateAsync(transport);
```

This connects the application to the running MCP Server.

---

## Fetch Available Tools

```csharp
var tools = await client.ListToolsAsync();
```

This retrieves all registered MCP tools.

Example

- get_products
- get_product
- create_product
- update_product
- delete_product
- get_orders
- create_address

---

## Register MCP Services

```csharp
builder.Services.AddSingleton(client);

builder.Services.AddSingleton<IReadOnlyList<McpClientTool>>(
    tools.AsReadOnly());
```

Now every service can access

- MCP Client
- Available MCP Tools

through Dependency Injection.

---

## Configure CORS

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

---

## Enable Swagger

```csharp
builder.Services.AddSwaggerGen();
```

---

# AzureOpenAIService

The service is responsible for

- Sending prompts to Azure OpenAI
- Providing MCP tools to the model
- Executing requested tools
- Returning the final AI response

---

## Dependencies

```csharp
private readonly ChatClient _chatClient;
private readonly McpClient _mcpClient;
private readonly IReadOnlyList<McpClientTool> _mcpTools;
```

---

## Initialize Azure OpenAI

```csharp
var azureClient = new AzureOpenAIClient(
    new Uri(settings.Endpoint),
    new ApiKeyCredential(settings.ApiKey));
```

---

## Create Chat Client

```csharp
_chatClient =
    azureClient.GetChatClient(settings.DeploymentName);
```

The ChatClient communicates with the deployed GPT model.

---

## Convert MCP Tools

Each MCP Tool is converted into an OpenAI Function Tool.

```csharp
ChatTool.CreateFunctionTool(...)
```

This allows Azure OpenAI to understand

- tool name
- description
- parameters
- JSON schema

---

## Add Tools

```csharp
options.Tools.Add(tool);
```

Now the GPT model is aware of every MCP tool.

---

## Send User Prompt

```csharp
CompleteChatAsync(messages, options)
```

The prompt is sent to Azure OpenAI along with all available tool definitions.

---

## Detect Tool Calls

If the model needs external data

```csharp
completion.FinishReason ==
ChatFinishReason.ToolCalls
```

Azure OpenAI returns one or more function calls.

Example

```
get_products

get_orders

get_address
```

---

## Deserialize Arguments

```csharp
JsonSerializer.Deserialize<Dictionary<string, object>>
```

Arguments generated by GPT are converted into a dictionary.

Example

```
{
    "productId":1
}
```

---

## Execute MCP Tool

```csharp
await _mcpClient.CallToolAsync(...)
```

The MCP Client sends the request to the MCP Server.

Example

```
get_product
```

↓

```
Product Service
```

↓

```
Database
```

↓

```
Response
```

---

## Return Tool Result

The tool result is added back into the conversation.

```csharp
messages.Add(
    new ToolChatMessage(...)
);
```

The GPT model now receives the actual database response.

---

## Generate Final Answer

Azure OpenAI is called again.

This time it already has

- Original prompt
- Tool result

It generates the final natural language response.

---

# Controller

The controller exposes the API endpoint.

```http
POST /api/chat
```

Request

```json
{
    "message":"Show all products"
}
```

Controller

```csharp
[HttpPost]
public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
{
    var response =
        await _azureOpenAIService.ChatAsync(request.Message);

    return Ok(new ChatResponse
    {
        Message = response
    });
}
```

---

# Complete Request Flow

```
User
   │
   ▼
Angular
   │
   ▼
ASP.NET Controller
   │
   ▼
AzureOpenAIService
   │
   ▼
Azure OpenAI
   │
   │
   ├───────────────No Tool Needed──────────────►
   │                                            │
   │                                            ▼
   │                                  Final Response
   │
   │
   └────────────Tool Required──────────────────►
                    │
                    ▼
             MCP Client
                    │
                    ▼
              MCP Server
                    │
                    ▼
              Business Logic
                    │
                    ▼
                Database
                    │
                    ▼
             Tool Response
                    │
                    ▼
             Azure OpenAI
                    │
                    ▼
             Final Response
                    │
                    ▼
               ASP.NET API
                    │
                    ▼
                 Angular
```

---

# Example

### User

```
Show all products
```

Azure OpenAI decides

```
get_products
```

MCP Server executes

```
SELECT * FROM Products
```

Returns

```json
[
  {
    "productId":1,
    "name":"Dell Laptop"
  },
  {
    "productId":3,
    "name":"Samsung TV"
  }
]
```

Azure OpenAI converts the JSON into a natural language response and returns it to the user.

---

# Key Features

- Azure OpenAI GPT-5 Mini integration
- Function Calling support
- Dynamic MCP Tool discovery
- Automatic tool execution
- Multi-turn tool calling loop
- Dependency Injection support
- Secure credential management using User Secrets
- Swagger support for testing APIs
- Angular frontend integration
- Extensible architecture for adding new MCP tools
