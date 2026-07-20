using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System.Text.Json;


var transportOptions = new HttpClientTransportOptions
{
    Endpoint = new Uri("http://localhost:5000")
};

var transport = new HttpClientTransport(transportOptions);

var client = await McpClient.CreateAsync(transport);

Console.WriteLine("Connected successfully.");

var tools = await client.ListToolsAsync();

foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name} - {tool.Description}");
}


var result = await client.CallToolAsync("get_products");

Console.WriteLine($"IsError: {result.IsError}");

var text = ((TextContentBlock)result.Content.First()).Text;

Console.WriteLine(text);