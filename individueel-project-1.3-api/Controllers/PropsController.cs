using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using individueel_project_1._3_api.Dto;
using Microsoft.AspNetCore.Authentication;
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
    public async Task<ActionResult<IEnumerable<Room>>> GetPropsAsync([FromQuery] Guid roomId, [FromQuery] Guid? propId)
    {
        var room = await roomRepository.GetRoomByIdAsync(roomId);
        
        var auth = await authorizationService.AuthorizeAsync(User, room, "RoomPolicy");
        
        if (!auth.Succeeded) return Forbid();
        
        if (propId is not null)
        {
            var result = await propRepository.GetPropByIdAsync(propId.Value);
            if (result is null) return NotFound();
            return Ok(result);
        }
        else
        {
            var result = await roomRepository.GetRoomByIdAsync(roomId);
            if (result is null) return NotFound();
            return Ok(result.Props);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddPropAsync([FromBody] PropCreateDto prop)
    {
        var id = await propRepository.AddPropAsync(prop);
        return CreatedAtRoute("GetProp", new { propId = id }, prop);
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePropAsync([FromQuery] Guid propId, [FromBody] PropUpdateDto prop)
    {
        var original = await propRepository.GetPropByIdAsync(propId);
        if (original is null) return NotFound();
        
        var auth = await authorizationService.AuthorizeAsync(User, original, "RoomPolicy");
        
        if (!auth.Succeeded) return Forbid();
        
        await propRepository.UpdatePropAsync(propId, prop);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePropAsync([FromQuery] Guid propId)
    {
        var original = await propRepository.GetPropByIdAsync(propId);
        if (original is null) return NotFound();
        
        var auth = await authorizationService.AuthorizeAsync(User, original, "RoomPolicy");
        
        if (!auth.Succeeded) return Forbid();
        
        if (await propRepository.GetPropByIdAsync(propId) is null) return NotFound();
        await propRepository.DeletePropAsync(propId);
        return NoContent();
    }
}
