using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

// [ApiController]
// [Route("api/[controller]")]
//todo see if this is rly needed
public class UserRoomsController(
    ILogger<UserRoomsController> logger,
    ICrudRepository<(string Username, Guid RoomId), UserRoom> userRoomRepository)
    : ControllerBase
{
    private readonly ILogger<UserRoomsController> _logger = logger;

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<UserRoom>>> GetUserRoomsAsync([FromQuery] string? username, [FromQuery] Guid? roomId)
    {
        if (username == null ^ roomId == null)
        {
            return BadRequest("Bad Request: Either username and roomId have to both be left empty or both be entered. You entered only " + username == null ? "roomId" : "username");
        }
        if (username is null)
        {
            return Ok(await userRoomRepository.GetAllAsync());
        }
        else
        {
            var userRoom = await userRoomRepository.GetByIdAsync((username, roomId!.Value));
            return userRoom == null ? NotFound() : Ok(userRoom);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddUserRoom([FromBody] UserRoom userRoom)
    {
        if (await userRoomRepository.GetByIdAsync((userRoom.Username, userRoom.RoomId)) != null) return Conflict();
        await userRoomRepository.AddAsync(userRoom);
        return CreatedAtRoute("GetUsers", new { username = userRoom.Username, roomId = userRoom.RoomId }, userRoom);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserRoom([FromQuery] string username, [FromQuery] Guid roomId, [FromBody] UserRoom userRoom)
    {
        if (await userRoomRepository.GetByIdAsync((username, roomId)) == null) return NotFound();
        if (userRoom.Username != username || userRoom.RoomId != roomId) return BadRequest("Bad Request: You cannot change the username or the roomId.");
        await userRoomRepository.UpdateAsync((username, roomId), userRoom);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserRoom([FromQuery] string username, [FromQuery] Guid roomId)
    {
        if (await userRoomRepository.GetByIdAsync((username, roomId)) == null) return NotFound();
        await userRoomRepository.DeleteAsync((username, roomId));
        return NoContent();
    }
}
