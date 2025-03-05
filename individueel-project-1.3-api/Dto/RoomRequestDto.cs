namespace individueel_project_1._3_api.Dto;

public class RoomRequestDto
{
    public required Guid RoomId { get; init; }
    public required string Name { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string TileId { get; init; }
    public bool? IsOwner { get; init; }
    public List<PropRequestDto> Props { get; init; } = [];
}