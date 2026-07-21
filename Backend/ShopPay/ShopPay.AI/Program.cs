using ModelContextProtocol.Client;
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
}