using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Options;
using System.ClientModel;
using System.Text.Json;
using ShopPay.AI;
using ModelContextProtocol.Client;

public class AzureOpenAIService
{
    private readonly ChatClient _chatClient;
    private readonly McpClient _mcpClient;
    private readonly IReadOnlyList<McpClientTool> _mcpTools;

    public AzureOpenAIService(IOptions<AzureOpenAISettings> options, McpClient mcpClient, IReadOnlyList<McpClientTool> tools)
    {
        var settings = options.Value;

        var azureClient = new AzureOpenAIClient(
            new Uri(settings.Endpoint),
            new ApiKeyCredential(settings.ApiKey));

        _chatClient = azureClient.GetChatClient(settings.DeploymentName);

        _mcpClient = mcpClient;
        _mcpTools = tools;
    }

    public async Task<string> ChatAsync(string prompt)
    {

        List<ChatTool> chatTools = new();

        if (_mcpTools != null)
        {
            foreach (var tool in _mcpTools)
            {
                var schema = tool.JsonSchema;

                chatTools.Add(ChatTool.CreateFunctionTool(
                    tool.Name,
                    tool.Description,
                    BinaryData.FromObjectAsJson(schema)
                ));
            }
        }

        List<ChatMessage> messages = [new UserChatMessage(prompt)];

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
                messages.Add(new AssistantChatMessage(completion));

                foreach (var toolCall in completion.ToolCalls)
                {
                    var arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(toolCall.FunctionArguments.ToString());

                    var toolResult = await _mcpClient.CallToolAsync(toolCall.FunctionName, arguments);

                    messages.Add(new ToolChatMessage(toolCall.Id, JsonSerializer.Serialize(toolResult.Content)));
                }
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