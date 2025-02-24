namespace individueel_project_1._3_api.Dto;

public class UserRoomCreateDto
{
    public required string Username { get; init; }
    public required Guid RoomId { get; init; }
    public required bool IsOwner { get; init; }
}