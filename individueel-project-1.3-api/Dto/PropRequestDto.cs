namespace individueel_project_1._3_api.Dto;

public class PropRequestDto
{
    public Guid PropId { get; set; }
    public string PrefabId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float Rotation { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public int SortingLayer { get; set; }
    public Guid RoomId { get; set; }

}