using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoomsController(
    ILogger<UserRoomsController> logger,
    IUserRoomRepository userRoomRepository)
    : ControllerBase
{
    private readonly ILogger<UserRoomsController> _logger = logger;

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<UserRoomRequestDto>>> GetUserRoomsAsync([FromQuery] Guid? roomId)
    {
        var user = User.Identity?.Name!;
        
        if (roomId is null)
        {
            return Ok(await userRoomRepository.GetUserRoomsByUserAsync(user));
        }

        var userRoom = await userRoomRepository.GetUserRoomAsyncById(user, roomId.Value);
        if (userRoom is null) return NotFound();
        return Ok(userRoom);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddUserRoom([FromBody] UserRoomCreateDto userRoom)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(userRoom.Username, userRoom.RoomId) != null) return Conflict();
        
        await userRoomRepository.AddUserRoomAsync(userRoom);
        
        return CreatedAtRoute("GetUsers", new { username = userRoom.Username, roomId = userRoom.RoomId }, userRoom);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserRoom([FromQuery] string username, [FromQuery] Guid roomId, [FromBody] UserRoomUpdateDto userRoom)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        await userRoomRepository.UpdateUserRoomAsync(username, roomId, userRoom);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserRoom([FromQuery] string username, [FromQuery] Guid roomId)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        await userRoomRepository.DeleteUserRoomAsync(username, roomId);
        return NoContent();
    }
}
