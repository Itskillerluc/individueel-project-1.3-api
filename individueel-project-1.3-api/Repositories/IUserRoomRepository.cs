using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Repositories;

public interface IUserRoomRepository
{
    Task<UserRoomRequestDto?> GetUserRoomAsyncById(string username, Guid roomId);
    Task<IEnumerable<UserRoomRequestDto>> GetUserRoomsByUserAsync(string username);
    Task<IEnumerable<UserRoomRequestDto>> GetUserRoomsByRoomAsync(Guid roomId);
    Task<(string username, Guid roomId)> AddUserRoomAsync(UserRoomCreateDto userRoomCreateDto);
    Task DeleteUserRoomAsync(string username, Guid roomId);
    Task UpdateUserRoomAsync(string username, Guid roomId, UserRoomUpdateDto userRoomUpdateDto);
}