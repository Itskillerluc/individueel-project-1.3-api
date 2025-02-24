using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Models;

public class UserRoom
{
    public required string Username { get; init; }
    public required Guid RoomId { get; init; }
    public required bool IsOwner { get; init; }

    public UserRoomRequestDto ToDto()
    {
        return new UserRoomRequestDto
        {
            Username = Username,
            RoomId = RoomId,
            IsOwner = IsOwner
        };
    }
}
