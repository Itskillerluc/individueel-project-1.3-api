using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ILogger<UsersController> logger, ICrudRepository<string, User> userRepository)
    : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<(bool, string)>>> GetUsersAsync([FromQuery] string? username)
    {
        if (username is null)
        {
            var user = await userRepository.GetAllAsync();
            return Ok(user);
        }
        else
        {
            var user = await userRepository.GetByIdAsync(username);
            return user == null ? NotFound() : Ok(user);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        if (await userRepository.GetByIdAsync(user.Username) != null) return Conflict();
        await userRepository.AddAsync(user);
        return CreatedAtRoute("GetUsers", new { username = user.Username }, user);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromQuery] string username, [FromBody] User user)
    {
        if (await userRepository.GetByIdAsync(username) == null) return NotFound();
        if (!user.Username.Equals(username)) return BadRequest("Bad Request: You cannot change the username.");
        await userRepository.UpdateAsync(username, user);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser([FromQuery] string username)
    {
        //todo: find out what this exception would be and write a test for it when i find out
        try
        {
            if (await userRepository.GetByIdAsync(username) == null) return NotFound();
            await userRepository.DeleteAsync(username);
            return NoContent();
        } catch (SqlException exception)
        {
            if (exception.ErrorCode == -2146232060)
            {
                return BadRequest("Bad Request: " + exception.Message.Replace("The statement has been terminated.", ""));
            }
            throw;
        }
        
    }
}
