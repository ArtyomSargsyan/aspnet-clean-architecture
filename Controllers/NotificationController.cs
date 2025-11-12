using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationManager _manager;

    public NotificationController(INotificationManager manager)
    {
        _manager = manager;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] NotificationDto dto)
    {
        await _manager.SendAsync(dto);
        return Ok("Notification sent!");
    }
}
