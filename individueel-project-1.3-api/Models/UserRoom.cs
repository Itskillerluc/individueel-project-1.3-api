using System.ComponentModel.DataAnnotations;

namespace individueel_project_1._3_api.Models;

public class UserRoom(string username, Guid roomId, bool isOwner)
{
    [Required]
    public string Username { get; set; } = username;
    [Required]
    public Guid RoomId { get; set; } = roomId;
    [Required]
    public bool IsOwner { get; set; } = isOwner;
}
