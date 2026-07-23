using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ShopPay.AI.Api.Interfaces;

namespace ShopPay.AI.Api.Services;

public class ChatService
{
    private readonly IAIService _aiService;
    private readonly McpClient _client;
    private readonly IReadOnlyList<McpClientTool> _tools;

    public ChatService(IAIService aiService, McpClient client, IReadOnlyList<McpClientTool> tools)
    {
        _aiService = aiService;
        _client = client;
        _tools = tools;
    }

    public async Task<string> ChatAsync(string prompt)
    {
        // Ask the LLM whether an MCP tool should be called
        var response = await _aiService.ChatAsync(prompt, _tools);

        // If no tool is required, return the LLM response directly
        if (!response.StartsWith("TOOL:", StringComparison.OrdinalIgnoreCase))
        {
            return response;
        }

        // Extract tool name
        var toolName = response.Replace("TOOL:", "", StringComparison.OrdinalIgnoreCase)
                               .Trim();

        // Execute MCP tool
        var toolResponse = await _client.CallToolAsync(toolName);

        var toolResult = ((TextContentBlock)toolResponse.Content.First()).Text;

        // Ask the LLM to convert the tool result into a user-friendly response
        return await _aiService.GenerateFinalResponseAsync(
            prompt,
            toolResult);
    }
}