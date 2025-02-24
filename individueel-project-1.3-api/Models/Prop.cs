using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Models;

public class Prop
{
    public required Guid PropId { get; init; }
    public required string PrefabId { get; init; }
    public required float PosX { get; init; }
    public required float PosY { get; init; }
    public required float Rotation { get; init; }
    public required float ScaleX { get; init; }
    public required float ScaleY { get; init; }
    public required int SortingLayer { get; init; }
    public required Guid RoomId { get; init; }

    public PropRequestDto ToDto()
    {
        return new PropRequestDto
        {
            PropId = PropId,
            PrefabId = PrefabId,
            PosX = PosX,
            PosY = PosY,
            Rotation = Rotation,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            SortingLayer = SortingLayer,
            RoomId = RoomId
        };
    }
}
