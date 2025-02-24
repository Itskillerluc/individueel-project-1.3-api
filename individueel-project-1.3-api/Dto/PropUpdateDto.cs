namespace individueel_project_1._3_api.Dto;

public class PropUpdateDto
{
    public required string PrefabId { get; init; }
    public required float PosX { get; init; }
    public required float PosY { get; init; }
    public required float Rotation { get; init; }
    public required float ScaleX { get; init; }
    public required float ScaleY { get; init; }
    public required int SortingLayer { get; init; }
}