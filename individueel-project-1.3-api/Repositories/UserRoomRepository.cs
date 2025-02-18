using Dapper;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class UserRoomRepository(string connectionString) : ICrudRepository<(string Username, Guid RoomId), UserRoom>
{
    private readonly string _connectionString = connectionString;

    public async Task<bool> AddAsync(UserRoom entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var result = await connection.ExecuteAsync("INSERT INTO dbo.[User_Room] (Username, RoomId, IsOwner) VALUES (@username, @roomId, @isOwner)", new { username = entity.Username, roomId = entity.RoomId, isOwner = entity.IsOwner });
        return result == 1;
    }

    public async Task<bool> DeleteAsync((string Username, Guid RoomId) id)
    {
        using var connection = new SqlConnection(_connectionString);

        var result = await connection.ExecuteAsync("DELETE FROM dbo.[User_Room] WHERE Username = @username AND RoomId = @roomId", new { username = id.Username, roomId = id.RoomId });

        return result == 1;
    }

    public async Task<IEnumerable<UserRoom>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryAsync<UserRoom>("SELECT Username, RoomId, IsOwner FROM dbo.[User_Room]");
    }

    public async Task<UserRoom?> GetByIdAsync((string Username, Guid RoomId) id)
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<UserRoom>("SELECT Username, RoomId, IsOwner FROM dbo.[User_Room] WHERE Username = @username AND RoomId = @roomId", new { username = id.Username, roomId = id.RoomId });
    }

    public async Task<bool> UpdateAsync((string Username, Guid RoomId) id, UserRoom updated)
    {
        using var connection = new SqlConnection(_connectionString);

        var result = await connection.ExecuteAsync("UPDATE dbo.[User_Room] SET IsOwner = @isOwner WHERE Username = @username AND RoomId = @roomId", new { username = id.Username, roomId = id.RoomId, isOwner = updated.IsOwner });

        return result == 1;
    }
}
