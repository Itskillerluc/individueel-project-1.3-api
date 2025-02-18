using Dapper;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class RoomRepository(string connectionString) : ICrudRepository<Guid, Room>
{
    private readonly string _connectionString = connectionString;

    public async Task<bool> AddAsync(Room entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("INSERT INTO dbo.[Room] VALUES (RoomId, Name, Width, Height, TileId) = (@roomId, @name, @width, @height, @tileId)", new { roomId = entity.RoomId, name = entity.Name, width = entity.Width, height = entity.Height });

        return effected == 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("DELETE FROM dbo.[Room] WHERE RoomId = @roomId", new { roomId = id });

        return effected == 1;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @"SELECT
					r.RoomId,
					r.[Name],
					r.Width,
                    r.Height,
                    r.TileId,
					ISNULL(ur.IsOwner, 0) AS 'IsOwner',
					ISNULL(u.Username, '') AS 'Username',
					ISNULL(u.[Password], '') AS 'Password',
					ISNULL(p.PropId, '00000000-0000-0000-0000-000000000000') AS 'PropId',
					ISNULL(p.PrefabId, '') AS 'PrefabId',
					ISNULL(p.PosX, 0) AS 'PosX',
					ISNULL(p.PosY, 0) AS 'PosY',
					ISNULL(p.Rotation, 0) AS 'Rotation',
					ISNULL(p.ScaleX, 0) AS 'ScaleX',
					ISNULL(p.ScaleY, 0) AS 'ScaleY',
					ISNULL(p.SortingLayer, 0) AS 'SortingLayer',
                    ISNULL(p.RoomId, '00000000-0000-0000-0000-000000000000') AS 'RoomId'
                    FROM Room r
					LEFT JOIN User_Room ur ON r.RoomId = ur.RoomId
					LEFT JOIN [User] u ON ur.Username = u.Username
					LEFT JOIN Prop p ON r.RoomId = p.RoomId";

        var result = connection.Query(sql);

        var rooms = await connection.QueryAsync<Room, bool, User, Prop, Room>(sql, (room, canEdit, user, prop) =>
        {
            if (user == null) return room;
            room.Users.Add(new(user, canEdit));
            room.Props.Add(prop);
            return room;
        }, splitOn: "IsOwner, Username, PropId");

        return MergeRooms(rooms);
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"SELECT
					r.RoomId,
					r.[Name],
					r.Width,
                    r.Height,
                    r.TileId,
					ISNULL(ur.IsOwner, 0) AS 'IsOwner',
					ISNULL(u.Username, '') AS 'Username',
					ISNULL(u.[Password], '') AS 'Password',
					ISNULL(p.PropId, '00000000-0000-0000-0000-000000000000') AS 'PropId',
					ISNULL(p.PrefabId, '') AS 'PrefabId',
					ISNULL(p.PosX, 0) AS 'PosX',
					ISNULL(p.PosY, 0) AS 'PosY',
					ISNULL(p.Rotation, 0) AS 'Rotation',
					ISNULL(p.ScaleX, 0) AS 'ScaleX',
					ISNULL(p.ScaleY, 0) AS 'ScaleY',
					ISNULL(p.SortingLayer, 0) AS 'SortingLayer',
                    p.RoomId
                    FROM Room r
					LEFT JOIN User_Room ur ON r.RoomId = ur.RoomId
					LEFT JOIN [User] u ON ur.Username = u.Username
					LEFT JOIN Prop p ON r.RoomId = p.RoomId
                    WHERE r.RoomId = @roomId";

        var rooms = await connection.QueryAsync<Room, bool, User, Room>(sql, (room, canEdit, user) =>
        {
            if (user == null) return room;
            room.Users.Add(new(user, canEdit));
            return room;
        }, new { roomId = id }, splitOn: "IsOwner, Username, PropId");

        return MergeRooms(rooms).FirstOrDefault();
    }

    public async Task<bool> UpdateAsync(Guid id, Room room)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("UPDATE dbo.[Room] SET Name = @name, Width = @width, Height = @height, TileId = @tileId WHERE RoomId = @roomId", new { roomId = room.RoomId, name = room.Name, width = room.Width, height = room.Height, tileId = room.TileId });

        return effected == 1;
    }

    private static List<Room> MergeRooms(IEnumerable<Room> rooms)
    {
        return [.. rooms.GroupBy(room => room.RoomId).Select(group =>
        {
            if (!group.Any()) return null;
            var first = group.First();
            var room = new Room(first.RoomId, first.Name, first.Width, first.Height, first.TileId);

            foreach (Room groupRoom in group)
            {
                if (groupRoom.Users.Count != 0)
                {
                    room.Users.Add(groupRoom.Users.Single());
                }
                if (groupRoom.Props.Count != 0)
                {
                    room.Props.Add(groupRoom.Props.Single());
                }
            }

            room.Users = [.. room.Users.DistinctBy(user => user.User.Username)];
            room.Props = [.. room.Props.DistinctBy(prop => prop.PropId)];

            return room;
        }).OfType<Room>()];
    }
}
