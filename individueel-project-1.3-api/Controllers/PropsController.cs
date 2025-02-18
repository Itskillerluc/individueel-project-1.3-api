using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropsController(
    ILogger<PropsController> logger,
    ICrudRepository<Guid, Prop> propRepository,
    ICrudRepository<Guid, Room> roomRepository)
    : ControllerBase
{
    private readonly ILogger<PropsController> _logger = logger;

    [HttpGet(Name = "GetProp")]
    public async Task<ActionResult<IEnumerable<Room>>> GetPropsAsync([FromQuery] Guid? propId, [FromQuery] Guid? roomId)
    {
        if (propId is null && roomId is null) return BadRequest("Bad Request: propId and roomId cannot both be left empty.");
        Room? room = null;
        if (roomId is not null)
        {
            room = await roomRepository.GetByIdAsync(roomId.Value);
        }
        IEnumerable<Prop> props = roomId != null ? (await propRepository.GetAllAsync()).Where(prop =>
        {
            var props = room?.Props.Any(p => p.PropId.Equals(propId));
            return props != null && props.Value;
        }) : await propRepository.GetAllAsync();
        IEnumerable<Prop> result = propId != null ? props.Where(prop => prop.PropId.Equals(propId)) : props;
        
        if (propId != null && !result.Any()) return NotFound();
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddPropAsync([FromBody] Prop prop)
    {
        if (await propRepository.GetByIdAsync(prop.PropId) != null) return Conflict();
        await propRepository.AddAsync(prop);
        return CreatedAtRoute("GetProp", new { propId = prop.PropId }, prop);
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePropAsync([FromQuery] Guid propId, [FromBody] Prop prop)
    {
        if (await propRepository.GetByIdAsync(propId) == null) return NotFound();
        await propRepository.UpdateAsync(propId, prop);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePropAsync([FromQuery] Guid propId)
    {
        if (await propRepository.GetByIdAsync(propId) == null) return NotFound();
        await roomRepository.DeleteAsync(propId);
        return NoContent();
    }
}
