using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]

//todo: Ask "Should there be a limit of 5 rooms built into the api"
public class RoomsController(
    IRoomRepository roomRepository,
    IPropRepository propRepository,
    IUserRoomRepository userRoomRepository,
    IAuthorizationService authorizationService)
    : ControllerBase
{
    [HttpGet(Name = "GetRoom")]
    public async Task<ActionResult<IEnumerable<RoomRequestDto>>> GetRoomsAsync([FromQuery] Guid? id)
    {
        var user = User?.Identity;

        if (id == null)
        {
            return Ok(await roomRepository.GetRoomsByUserAsync(user?.Name!));
        }

        var result = await roomRepository.GetRoomByIdAsync(id.Value, user?.Name!);

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User!, result, "WeakRoomPolicy");

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddRoomAsync([FromBody] RoomCreateDto room)
    {
        var userName = User?.Identity?.Name!;

        if ((await roomRepository.GetRoomsByUserAsync(userName)).Count(r =>
                r.IsOwner ?? false) >= 5)
        {
            return BadRequest("A user cannot have more than 5 rooms");
        }

        if (await roomRepository.GetRoomByNameAndUserAsync(room.Name, userName) != null) return Conflict();

        var id = await roomRepository.AddRoomAsync(room);
        await userRoomRepository.AddUserRoomAsync(new UserRoomCreateDto
            { Username = userName, RoomId = id, IsOwner = true });

        return CreatedAtRoute("GetRoom", new { roomId = id }, room);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateRoomAsync([FromQuery] Guid roomId, [FromBody] RoomUpdateDto room)
    {
        var original = await roomRepository.GetRoomByIdAsync(roomId, User?.Identity?.Name!);
        if (original == null) return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User!, original, "StrictRoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();
        await roomRepository.UpdateRoomAsync(roomId, room);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRoomAsync([FromQuery] Guid roomId)
    {
        var original = await roomRepository.GetRoomByIdAsync(roomId, User?.Identity?.Name!);
        if (original == null) return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User!, original, "StrictRoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();

        await userRoomRepository.DeleteUserRoomsByRoomAsync(roomId);
        await propRepository.DeletePropsByRoomAsync(roomId);
        await roomRepository.DeleteRoomAsync(roomId);
        return NoContent();
    }
}