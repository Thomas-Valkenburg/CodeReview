using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using CodeReview.Core.Handlers;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private WebSocket? _webSocket;

    [HttpGet]
    public async Task<ActionResult> OpenWebsocketAsync()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            return BadRequest();
        }

        _webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

        PostHandler.OnPostCreated += SendNotificationAsync;

        while (_webSocket.State is WebSocketState.Connecting or WebSocketState.Open)
        {
            await Task.Delay(1000);
        }

        PostHandler.OnPostCreated -= SendNotificationAsync;

        return Ok();
    }

    private void SendNotificationAsync(object? obj, EventArgs args)
    {
        Task.Run(async () =>
        {
            if (_webSocket is null)
            {
                return;
            }

            var message = "New post created."u8.ToArray();
            await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
        });
    }
}