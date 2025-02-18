using Dapper;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class UserRepository(string connectionString) : ICrudRepository<string, User>
{
    private readonly string _connectionString = connectionString;

    public async Task<bool> AddAsync(User entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("INSERT INTO dbo.[User] (Username, Password) VALUES (@username, @password)", new { username = entity.Username, password = entity.Password });

        return effected == 1;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("DELETE FROM dbo.[User] WHERE Username = @username", new { username = id });

        return effected == 1;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @$"SELECT
					ISNULL(r.RoomId, '00000000-0000-0000-0000-000000000000') AS 'RoomId',
					ISNULL(r.[Name], '') AS 'Name',
					ISNULL(r.Width, 0) AS 'Width',
                    ISNULL(r.Height, 0) AS 'Height',
                    ISNULL(r.TileId, 0) AS 'TileId',
                    ISNULL(ur.IsOwner, 0) AS 'IsOwner',
                    u.Username,
                    u.[Password]
                    FROM [User] u
                    LEFT JOIN User_Room ur ON u.Username = ur.Username
                    LEFT JOIN Room r ON ur.RoomId = r.RoomId";

        var users = await connection.QueryAsync<Room, bool, User, User>(sql, (room, canEdit, user) =>
        {
            if (room == null || room.RoomId.Equals(Guid.Empty)) return user;
            user.Rooms.Add(new(room, canEdit));
            return user;
        }, splitOn: "IsOwner, Username");

        return MergeUsers(users) ?? [];
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @$"SELECT
					ISNULL(r.RoomId, '00000000-0000-0000-0000-000000000000') AS 'RoomId',
					ISNULL(r.[Name], '') AS 'Name',
					ISNULL(r.Width, 0) AS 'Width',
                    ISNULL(r.Height, 0) AS 'Height',
                    ISNULL(r.TileId, 0) AS 'TileId',
                    ISNULL(ur.IsOwner, 0) AS 'IsOwner',
                    u.Username,
                    u.[Password]
                    FROM [User] u
                    LEFT JOIN User_Room ur ON u.Username = ur.Username
                    LEFT JOIN Room r ON ur.RoomId = r.RoomId";

        var users = await connection.QueryAsync<Room, bool, User, User>(sql, (room, canEdit, user) =>
        {
            if (room == null || room.RoomId.Equals(Guid.Empty)) return user;
            user.Rooms.Add(new(room, canEdit));
            return user;
        }, new { username = id }, splitOn: "IsOwner, Username");

        return MergeUsers(users).FirstOrDefault();
    }

    public async Task<bool> UpdateAsync(string id, User user)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("UPDATE dbo.[User] SET Password = @password WHERE Username = @username", new { username = user.Username, password = user.Password });

        return effected == 1;
    }

    private static List<User> MergeUsers(IEnumerable<User> users)
    {
        return [.. users.GroupBy(user => user.Username).Select(group =>
        {
            if (!group.Any()) return null;
            var first = group.First();
            var user = new User(first.Username, first.Password);


            foreach (User groupUser in group)
            {
                if (groupUser.Rooms.Count == 0) continue;
                user.Rooms.Add(groupUser.Rooms.Single());
            }

            return user;
        }).OfType<User>()];
    }
}
