using System.ComponentModel.DataAnnotations;
using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Models;

public class Room
{

    public required Guid RoomId { get; set; }
    public required string Name { get; set; }
    public required float Width { get; set; }
    public required float Height { get; set; }
    public required string TileId { get; set; }

    public List<UserEntry> Users { get; set; } = [];

    public List<Prop> Props { get; set; } = [];

    public RoomRequestDto ToDto()
    {
        return new RoomRequestDto
        {
            RoomId = RoomId,
            Name = Name,
            Width = Width,
            Height = Height,
            TileId = TileId,
            Users = Users,
            Props = Props
        };
    }

    public record UserEntry([Required] string User, [Required] bool IsOwner);
}
