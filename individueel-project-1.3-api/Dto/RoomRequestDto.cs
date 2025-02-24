using individueel_project_1._3_api.Models;

namespace individueel_project_1._3_api.Dto;

public class RoomRequestDto
{
    public required Guid RoomId { get; init; }
    public required string Name { get; init; }
    public required float Width { get; init; }
    public required float Height { get; init; }
    public required string TileId { get; init; }
    public List<Room.UserEntry> Users { get; init; } = [];
    public List<PropRequestDto> Props { get; init; } = [];
}