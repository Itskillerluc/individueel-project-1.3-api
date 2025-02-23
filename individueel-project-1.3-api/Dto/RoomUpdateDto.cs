namespace individueel_project_1._3_api.Dto;


public class RoomUpdateDto
{
    public required string Name { get; init; }
    public required float Width { get; init; }
    public required float Height { get; init; }
    public required string TileId { get; init; }
}