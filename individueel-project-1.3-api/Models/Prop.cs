using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace individueel_project_1._3_api.Models;

public class Prop(Guid propId, string prefabId, float posX, float posY, float rotation, float scaleX, float scaleY, int sortinglayer, Guid roomId)
{
    [JsonConstructor]
    public Prop(Guid propId, string prefabId, double posX, double posY, double rotation, double scaleX, double scaleY, int sortingLayer, Guid roomId) : this(propId, prefabId, (float) posX, (float) posY, (float) rotation, (float) scaleX, (float)scaleY, sortingLayer, roomId)
    {

    }

    [Required]
    public Guid PropId { get; set; } = propId;
    [Required]
    public string PrefabId { get; set; } = prefabId;
    [Required]
    public float PosX { get; set; } = posX;
    [Required]
    public float PosY { get; set; } = posY;
    [Required]
    public float Rotation { get; set; } = rotation;
    [Required]
    public float ScaleX { get; set; } = scaleX;
    [Required]
    public float ScaleY { get; set; } = scaleY;
    [Required]
    public int SortingLayer { get; set; } = sortinglayer;
    [Required]
    public Guid RoomId { get; set; } = roomId;
}
