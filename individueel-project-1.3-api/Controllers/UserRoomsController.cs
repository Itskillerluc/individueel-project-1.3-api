using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoomsController(
    IRoomRepository roomRepository,
    IUserRoomRepository userRoomRepository,
    IAuthorizationService authorizationService)
    : ControllerBase
{

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<UserRoomRequestDto>>> GetUserRoomsAsync([FromQuery] Guid? roomId, [FromQuery] string? username)
    {
        //todo add null handling
        var user = User?.Identity?.Name!;
        
        if (roomId is null)
        {
            return Ok(await userRoomRepository.GetUserRoomsByUserAsync(user));
        }

        if (username is null)
        {
            return Ok(await userRoomRepository.GetUserRoomsByRoomAsync(roomId.Value));
        }
        
        var room = await roomRepository.GetRoomByIdAsync(roomId.Value);

        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "RoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();
        
        var userRoom = await userRoomRepository.GetUserRoomAsyncById(user, roomId.Value);
        if (userRoom is null) return NotFound();
        return Ok(userRoom);
    }
    
    
    [HttpPost]
    public async Task<ActionResult> AddUserRoom([FromBody] UserRoomCreateDto userRoom)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(userRoom.Username, userRoom.RoomId) != null) return Conflict();
        
        var room = await roomRepository.GetRoomByIdAsync(userRoom.RoomId);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "RoomPolicy");
        
        if (!authorizationResult.Succeeded) return Forbid();
        
        await userRoomRepository.AddUserRoomAsync(userRoom);
        
        return CreatedAtRoute("GetUsers", new { username = userRoom.Username, roomId = userRoom.RoomId }, userRoom);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateUserRoom([FromQuery] string username, [FromQuery] Guid roomId, [FromBody] UserRoomUpdateDto userRoom)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        
        var room = await roomRepository.GetRoomByIdAsync(roomId);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "RoomPolicy");
        
        if (!authorizationResult.Succeeded) return Forbid();
        
        await userRoomRepository.UpdateUserRoomAsync(username, roomId, userRoom);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserRoom([FromQuery] string username, [FromQuery] Guid roomId)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        
        var room = await roomRepository.GetRoomByIdAsync(roomId);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "RoomPolicy");
        
        if (!authorizationResult.Succeeded) return Forbid();
        
        await userRoomRepository.DeleteUserRoomAsync(username, roomId);
        return NoContent();
    }
}
