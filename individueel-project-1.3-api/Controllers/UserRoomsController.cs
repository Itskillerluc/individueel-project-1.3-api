using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
        
        var room = await roomRepository.GetRoomByIdAsync(roomId.Value, user);

        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();
        
        var userRoom = await userRoomRepository.GetUserRoomAsyncById(user, roomId.Value);
        if (userRoom is null) return NotFound();
        return Ok(userRoom);
    }
    
    
    [HttpPost]
    public async Task<ActionResult> AddUserRoom([FromBody] UserRoomCreateDto userRoom)
    {
        try
        {
            if (await userRoomRepository.GetUserRoomAsyncById(userRoom.Username, userRoom.RoomId) != null) return Conflict();
        
            var room = await roomRepository.GetRoomByIdAsync(userRoom.RoomId, User?.Identity?.Name!);
        
            var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");
        
            if (!authorizationResult.Succeeded) return Forbid();
        
            await userRoomRepository.AddUserRoomAsync(userRoom);

            //return okay so you cannot see if the user exists or not
            return Ok();
        } catch (SqlException exception)
        {
            if (exception.Number == 547) return Ok();
            throw;
        }
        
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateUserRoom([FromQuery] string username, [FromQuery] Guid roomId, [FromBody] UserRoomUpdateDto userRoom)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        
        var room = await roomRepository.GetRoomByIdAsync(roomId, User?.Identity?.Name!);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");
        
        if (!authorizationResult.Succeeded) return Forbid();
        
        await userRoomRepository.UpdateUserRoomAsync(username, roomId, userRoom);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserRoom([FromQuery] string username, [FromQuery] Guid roomId)
    {
        if (await userRoomRepository.GetUserRoomAsyncById(username, roomId) is null) return NotFound();
        
        var room = await roomRepository.GetRoomByIdAsync(roomId, User?.Identity?.Name!);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User!, room, "WeakRoomPolicy");
        
        if (!authorizationResult.Succeeded) return Forbid();
        
        await userRoomRepository.DeleteUserRoomAsync(username, roomId);
        return NoContent();
    }
}
