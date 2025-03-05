using System.ComponentModel.DataAnnotations;

namespace individueel_project_1._3_api.Dto;

public class RoomCreateDto
{
    [Length(1, 25)]
    public required string Name { get; init; }
    [Range(20, 200)]
    public required int Width { get; init; }
    [Range(10, 100)]
    public required int Height { get; init; }
    public required string TileId { get; init; }
}