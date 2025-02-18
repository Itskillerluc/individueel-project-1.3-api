using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace individueel_project_1._3_api.Models;

[method: JsonConstructor]
public class User(string username, string password)
{
    public User(string username, string password, List<RoomEntry> rooms) : this(username, password)
    {
        this.Username = username;
        this.Rooms = rooms;
    }

    [Required]
    public string Username { get; set; } = username;
    [Required]
    public string Password { get; set; } = password;

    public List<RoomEntry> Rooms { get; set; } = [];

    public record RoomEntry(Room room, bool IsOwner);
}
