using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelContextProtocol.Client;

namespace ShopPay.AI.Api.Interfaces;

public interface IAIService
{
    Task<string> ChatAsync(
        string prompt,
        IEnumerable<McpClientTool> tools);

    Task<string> GenerateFinalResponseAsync(string userPrompt, string toolResult);
}