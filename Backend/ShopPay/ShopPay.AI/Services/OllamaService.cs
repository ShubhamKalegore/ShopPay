using System.Net.Http.Json;
using System.Text.Json;
using ModelContextProtocol.Client;
using ShopPay.AI.Api.Interfaces;

namespace ShopPay.AI.Api.Services;

public class OllamaService : IAIService
{
    private readonly HttpClient _httpClient;

    public OllamaService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434")
        };
    }

    public async Task<string> ChatAsync(
        string prompt,
        IEnumerable<McpClientTool> tools)
    {
        var toolDescriptions = string.Join(
            Environment.NewLine,
            tools.Select(t => $"- {t.Name}: {t.Description}"));

        var request = new
        {
            model = "llama3.1",
            stream = false,
            messages = new object[]
            {
        new
        {
            role = "system",
            content = $$"""
            You are an AI assistant that decides whether to call MCP tools.

            Available MCP tools:
            {{toolDescriptions}}

            Rules:
            - If a tool is required, respond ONLY with:
              TOOL:<tool_name>
            - Do not explain anything.
            - Do not ask follow-up questions.
            - Do not return markdown.
            - If no tool is needed, answer the user's question normally.

            Examples:

            User: Show all products
            Assistant: TOOL:get_products

            User: List all orders
            Assistant: TOOL:get_orders

            User: Show all addresses
            Assistant: TOOL:get_addresses
            """
        },
        new
        {
            role = "user",
            content = prompt
        }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/chat", request);

        response.EnsureSuccessStatusCode();

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        return json.RootElement
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }

    public async Task<string> GenerateFinalResponseAsync(
    string userPrompt,
    string toolResult)
    {
        var request = new
        {
            model = "llama3.1",
            stream = false,
            messages = new object[]
            {
            new
            {
                role = "system",
                content = """
                You are a helpful AI assistant.

                A tool has already been executed.

                Your job is to answer the user's question using ONLY the tool result.

                Never mention tools.
                Never mention JSON.
                Present the information naturally.
                """
            },
            new
            {
                role = "user",
                content = $"""
                User Question:
                {userPrompt}

                Tool Result:
                {toolResult}

                Answer the user's question in a clear and friendly way.
                """
            }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/chat", request);

        response.EnsureSuccessStatusCode();

        using var json = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());

        return json.RootElement
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}