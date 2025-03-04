using System.ComponentModel.DataAnnotations;

namespace individueel_project_1._3_api.Dto;

public class RoomCreateDto
{
    [Length(1, 25)]
    public required string Name { get; init; }
    public required float Width { get; init; }
    public required float Height { get; init; }
    public required string TileId { get; init; }
}