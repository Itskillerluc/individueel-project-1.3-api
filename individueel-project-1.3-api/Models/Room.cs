using System.ComponentModel.DataAnnotations;
using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Models;

public class Room
{

    public required Guid RoomId { get; init; }
    public required string Name { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string TileId { get; init; }

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
            IsOwner = Users.SingleOrDefault()?.IsOwner,
            Props = Props.Select(prop => prop.ToDto()).ToList()
        };
    }

    public record UserEntry([Required] string User, [Required] bool IsOwner);
}
