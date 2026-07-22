using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Options;
using System.ClientModel;
using System.Text.Json;
using ShopPay.AI;
using ModelContextProtocol.Client;

public class AzureOpenAIService
{
    // 1. Declare the private fields at the top so they are "in context"
    private readonly ChatClient _chatClient;
    private readonly McpClient _mcpClient;
    private readonly IReadOnlyList<McpClientTool> _mcpTools;

    public AzureOpenAIService(
        IOptions<AzureOpenAISettings> options,
        McpClient mcpClient,
        IReadOnlyList<McpClientTool> tools)
    {
        var settings = options.Value;

        // Initialize the Azure client
        var azureClient = new AzureOpenAIClient(
            new Uri(settings.Endpoint),
            new ApiKeyCredential(settings.ApiKey));

        // We store the ChatClient directly so we don't need _client or _deploymentName later
        _chatClient = azureClient.GetChatClient(settings.DeploymentName);

        _mcpClient = mcpClient;
        _mcpTools = tools;
    }

    public async Task<string> ChatAsync(string prompt)
    {
        // 2. We use _chatClient directly (it was created in the constructor)

        // 3. Convert MCP Tools to OpenAI ChatTools
        List<ChatTool> chatTools = new();

        if (_mcpTools != null)
        {
            foreach (var tool in _mcpTools)
            {
                // Use JsonSchema as seen in your IntelliSense screenshot
                var schema = tool.JsonSchema;

                chatTools.Add(ChatTool.CreateFunctionTool(
                    tool.Name,
                    tool.Description,
                    BinaryData.FromObjectAsJson(schema)
                ));
            }
        }

        List<ChatMessage> messages = [new UserChatMessage(prompt)];

        // 4. Correctly add tools to the options
        ChatCompletionOptions options = new();
        foreach (var t in chatTools)
        {
            options.Tools.Add(t);
        }

        bool isProcessing = true;
        string result = "";

        while (isProcessing)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                // Add the AI's request to the conversation history
                messages.Add(new AssistantChatMessage(completion));

                foreach (var toolCall in completion.ToolCalls)
                {
                    // Deserialize arguments to pass to MCP
                    var arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(toolCall.FunctionArguments.ToString());

                    // Execute tool via MCP
                    var toolResult = await _mcpClient.CallToolAsync(toolCall.FunctionName, arguments);

                    // Add result back to AI history
                    messages.Add(new ToolChatMessage(toolCall.Id, JsonSerializer.Serialize(toolResult.Content)));
                }
                // Loop continues back to 'CompleteChatAsync' so the AI can process the tool results
            }
            else
            {
                result = completion.Content[0].Text;
                isProcessing = false;
            }
        }

        return result;
    }
}