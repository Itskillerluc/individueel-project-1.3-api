using Dapper;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class UserRoomRepository(string connectionString) : IUserRoomRepository
{
    public async Task<UserRoomRequestDto?> GetUserRoomAsyncById(string username, Guid roomId)
    {
        
        await using var connection = new SqlConnection(connectionString);

        return (await connection.QueryFirstOrDefaultAsync<UserRoom>("SELECT Username, RoomId, IsOwner FROM dbo.[User_Room] WHERE Username = @username AND RoomId = @roomId", 
            new { username, roomId }))?.ToDto();
    }

    public async Task<IEnumerable<UserRoomRequestDto>> GetUserRoomsByUserAsync(string username)
    {
        await using var connection = new SqlConnection(connectionString);

        return (await connection.QueryAsync<UserRoom>("SELECT Username, RoomId, IsOwner FROM dbo.[User_Room] WHERE Username = @username", 
            new { username })).Select(userRoom => userRoom.ToDto());
    }

    public async Task<IEnumerable<UserRoomRequestDto>> GetUserRoomsByRoomAsync(Guid roomId)
    {
        await using var connection = new SqlConnection(connectionString);

        return (await connection.QueryAsync<UserRoom>("SELECT Username, RoomId, IsOwner FROM dbo.[User_Room] WHERE RoomId = @roomId", 
            new { roomId })).Select(userRoom => userRoom.ToDto());
    }

    public async Task<(string username, Guid roomId)> AddUserRoomAsync(UserRoomCreateDto userRoomCreateDto)
    {
        await using var connection = new SqlConnection(connectionString);
        
        await connection.ExecuteAsync("INSERT INTO dbo.[User_Room] (Username, RoomId, IsOwner) VALUES (@username, @roomId, @isOwner)", 
            new { username = userRoomCreateDto.Username, roomId = userRoomCreateDto.RoomId, isOwner = userRoomCreateDto.IsOwner });
        
        return (userRoomCreateDto.Username, userRoomCreateDto.RoomId);
    }

    public async Task DeleteUserRoomAsync(string username, Guid roomId)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("DELETE FROM dbo.[User_Room] WHERE Username = @username AND RoomId = @roomId",
            new { username, roomId });
    }

    public async Task DeleteUserRoomsByRoomAsync(Guid roomId)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("DELETE FROM dbo.[User_Room] WHERE RoomId = @roomId",
            new { roomId });
    }

    public async Task UpdateUserRoomAsync(string username, Guid roomId, UserRoomUpdateDto userRoomUpdateDto)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("UPDATE dbo.[User_Room] SET IsOwner = @isOwner WHERE Username = @username AND RoomId = @roomId",
            new { username, roomId, isOwner = userRoomUpdateDto.IsOwner });
    }
}
