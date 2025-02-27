﻿using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]

//todo: Ask "Should there be a limit of 5 rooms built into the api"
public class RoomsController(
    IRoomRepository roomRepository,
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

        var result = await roomRepository.GetRoomByIdAsync(id.Value);

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User!, result, "RoomPolicy");

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

        var id = await roomRepository.AddRoomAsync(room);
        await userRoomRepository.AddUserRoomAsync(new UserRoomCreateDto
            { Username = userName, RoomId = id, IsOwner = true });

        return CreatedAtRoute("GetRoom", new { roomId = id }, room);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateRoomAsync([FromQuery] Guid roomId, [FromBody] RoomUpdateDto room)
    {
        var original = await roomRepository.GetRoomByIdAsync(roomId);
        if (original == null) return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, original, "RoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();
        await roomRepository.UpdateRoomAsync(roomId, room);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRoomAsync([FromQuery] Guid roomId)
    {
        var original = await roomRepository.GetRoomByIdAsync(roomId);
        if (original == null) return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, original, "RoomPolicy");

        if (!authorizationResult.Succeeded) return Forbid();

        await roomRepository.DeleteRoomAsync(roomId);
        return NoContent();
    }
}