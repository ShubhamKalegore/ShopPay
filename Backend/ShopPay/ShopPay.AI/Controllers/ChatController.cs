using Microsoft.AspNetCore.Mvc;
using ShopPay.AI.Api.Models;
using ShopPay.AI.Api.Services;

namespace ShopPay.AI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    //private readonly ChatService _chatService;
    private readonly AzureOpenAIService _azureOpenAIService;

    public ChatController(AzureOpenAIService azureOpenAIService)
    {
        //_chatService = chatService;
        _azureOpenAIService = azureOpenAIService;
    }

    //[HttpPost]
    //public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    //{
    //    var response = await _chatService.ChatAsync(request.Message);

    //    return Ok(new ChatResponse
    //    {
    //        Message = response
    //    });
    //}

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        var response = await _azureOpenAIService.ChatAsync(request.Message);

        return Ok(new ChatResponse
        {
            Message = response
        });
    }

    //[HttpGet("test")]
    //public async Task<IActionResult> Test()
    //{
    //    var response = await _azureOpenAIService.ChatAsync("Hello");
    //    return Ok(response);
    //}
}

