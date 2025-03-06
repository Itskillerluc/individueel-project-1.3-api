using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using individueel_project_1._3_api.Dto;
using Microsoft.AspNetCore.Authorization;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropsController(
    IPropRepository propRepository,
    IRoomRepository roomRepository,
    IAuthorizationService authorizationService)
    : ControllerBase
{
    [HttpGet(Name = "GetProp")]
    public async Task<ActionResult<IEnumerable<PropRequestDto>>> GetPropsAsync([FromQuery] Guid roomId,
        [FromQuery] Guid? propId)
    {
        var name = User?.Identity?.Name!;

        var room = await roomRepository.GetRoomByIdAsync(roomId, name);

        var auth = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");

        if (!auth.Succeeded) return Forbid();

        if (propId is not null)
        {
            var result = await propRepository.GetPropByIdAsync(propId.Value);
            if (result is null) return NotFound();
            return Ok(result);
        }
        else
        {
            var result = await roomRepository.GetRoomByIdAsync(roomId, name);
            if (result is null) return NotFound();
            return Ok(result.Props);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddPropAsync([FromBody] PropCreateDto prop)
    {
        var room = await roomRepository.GetRoomByIdAsync(prop.RoomId, User?.Identity?.Name!);

        var auth = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");

        if (!auth.Succeeded) return Forbid();

        var id = await propRepository.AddPropAsync(prop);
        return CreatedAtRoute("GetProp", new { propId = id }, prop);
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePropAsync([FromQuery] Guid propId, [FromBody] PropUpdateDto prop)
    {
        var original = await propRepository.GetPropByIdAsync(propId);
        if (original is null) return NotFound();

        var auth = await authorizationService.AuthorizeAsync(User, original, "StrictRoomPolicy");

        if (!auth.Succeeded) return Forbid();

        await propRepository.UpdatePropAsync(propId, prop);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePropAsync([FromQuery] Guid? propId, [FromQuery] Guid roomId)
    {
        if (propId != null)
        {
            var original = await propRepository.GetPropByIdAsync(propId.Value);
            if (original is null) return NotFound();

            var room = await roomRepository.GetRoomByPropAsync(propId.Value, User?.Identity?.Name!);

            var auth = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");

            if (!auth.Succeeded) return Forbid();

            if (await propRepository.GetPropByIdAsync(propId.Value) is null) return NotFound();
            await propRepository.DeletePropAsync(propId.Value);
        }
        else
        {
            var room = await roomRepository.GetRoomByIdAsync(roomId, User?.Identity?.Name!);
            if (room is null) return NotFound();

            var authorize = await authorizationService.AuthorizeAsync(User!, room, "StrictRoomPolicy");

            if (!authorize.Succeeded) return Forbid();

            await propRepository.DeletePropsByRoomAsync(roomId);
        }

        return NoContent();
    }
}