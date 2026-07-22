using ModelContextProtocol.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopPay.AI.Api.Interfaces;
using ShopPay.AI.Api.Services;
using Microsoft.AspNetCore.Hosting;
using ShopPay.AI;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(
    "http://localhost:7000",
    "https://localhost:7001");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Create MCP Client once
var transportOptions = new HttpClientTransportOptions
{
    Endpoint = new Uri("http://localhost:5000")
};

var transport = new HttpClientTransport(transportOptions);

var client = await McpClient.CreateAsync(transport);

var tools = await client.ListToolsAsync();

builder.Services.AddSingleton(client);
builder.Services.AddSingleton<IReadOnlyList<McpClientTool>>(tools.AsReadOnly());

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

builder.Services.Configure<AzureOpenAISettings>(
    builder.Configuration.GetSection("AzureOpenAI"));
builder.Services.AddScoped<AzureOpenAIService>();
var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors("Angular"); 

app.MapControllers();

app.Run();


/*using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ShopPay.AI.Services;

var transportOptions = new HttpClientTransportOptions
{
    Endpoint = new Uri("http://localhost:5000")
};

var transport = new HttpClientTransport(transportOptions);

var client = await McpClient.CreateAsync(transport);

var tools = await client.ListToolsAsync();

Console.WriteLine("Connected to MCP Server.");

Console.WriteLine("\nAvailable Tools:");
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name} - {tool.Description}");
}

var ollama = new OllamaService();

while (true)
{
    Console.Write("\nYou: ");

    var prompt = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(prompt))
        continue;

    if (prompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var response = await ollama.ChatAsync(prompt, tools);

    Console.WriteLine($"\nOllama Response: {response}");

    if (response.StartsWith("TOOL:"))
    {
        var toolName = response.Replace("TOOL:", "").Trim();

        var toolResult = await client.CallToolAsync(toolName);

        var text = ((TextContentBlock)toolResult.Content.First()).Text;

        Console.WriteLine($"\nTool Result:\n{text}");
    }
    else
    {
        Console.WriteLine($"\nAI: {response}");
    }
}*/