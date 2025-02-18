using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(ILogger<RoomsController> logger, ICrudRepository<Guid, Room> roomRepository)
    : ControllerBase
{
    private readonly ILogger<RoomsController> _logger = logger;


    [HttpGet(Name = "GetRoom")]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsAsync([FromQuery] Guid? id, [FromQuery] string? owner)
    {
        if (id is null && owner is null)
        {
            return Ok(await roomRepository.GetAllAsync());
        }
        IEnumerable<Room> rooms = owner != null ? (await roomRepository.GetAllAsync()).Where(room => room.Users.Any(usr => usr.IsOwner)) : await roomRepository.GetAllAsync();
        IEnumerable<Room> result = id != null ? rooms.Where(room => room.RoomId.Equals(id)) : rooms;
        
        if (id != null && !result.Any()) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddRoomAsync([FromBody] Room room)
    {
        //Todo: add validation
        if (await roomRepository.GetByIdAsync(room.RoomId) != null) return Conflict();
        await roomRepository.AddAsync(room);
        return CreatedAtRoute("GetRoom", new { roomId = room.RoomId }, room);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateRoomAsync([FromQuery] Guid roomId, [FromBody] Room room)
    {
        if (await roomRepository.GetByIdAsync(roomId) == null) return NotFound();
        await roomRepository.UpdateAsync(roomId, room);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRoomAsync([FromQuery] Guid roomId)
    {
        if (await roomRepository.GetByIdAsync(roomId) == null) return NotFound();
        await roomRepository.DeleteAsync(roomId);
        return NoContent();
    }
}
