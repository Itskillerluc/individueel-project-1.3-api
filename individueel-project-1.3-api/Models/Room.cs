using individueel_project_1._3_api.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace individueel_project_1._3_api.Models;

public class Room(Guid roomId, string name, float width, float height, string tileId)
{
    [JsonConstructor]

    public Room(Guid roomId, string name, double width, double height, string tileId) : this(roomId, name, (float) width, (float) height, tileId)
    {

    }

    [Required]
    public Guid RoomId { get; set; } = roomId;
    [Required]
    public string Name { get; set; } = name;
    [Required]
    public float Width { get; set; } = width;
    [Required]
    public float Height { get; set; } = height;
    [Required] 
    public string TileId { get; set; } = tileId;
    public List<UserEntry> Users { get; set; } = [];
    public List<Prop> Props { get; set; } = [];

    public record UserEntry(User User, bool IsOwner);
}
