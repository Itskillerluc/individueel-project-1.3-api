namespace individueel_project_1._3_api.Dto;

public class PropCreateDto
{
    public required string PrefabId { get; set; }
    public required float PosX { get; set; }
    public required float PosY { get; set; }
    public required float Rotation { get; set; }
    public required float ScaleX { get; set; }
    public required float ScaleY { get; set; }
    public required int SortingLayer { get; set; }
    public required Guid RoomId { get; set; }
}