using Microsoft.AspNetCore.Mvc;
using ShopPay.AI.Api.Models;
using ShopPay.AI.Api.Services;

namespace ShopPay.AI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        var response = await _chatService.ChatAsync(request.Message);

        return Ok(new ChatResponse
        {
            Message = response
        });
    }
}