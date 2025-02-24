using System.Security.Principal;
using Dapper;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class RoomRepository(string connectionString) : IRoomRepository
{
	public async Task<IEnumerable<RoomRequestDto>> GetRoomsByUserAsync(string username)
    {
	    await using var connection = new SqlConnection(connectionString);
	    const string sql = @"SELECT
					r.RoomId,
					r.[Name],
					r.Width,
                    r.Height,
                    r.TileId,
					ISNULL(ur.IsOwner, 0) AS 'IsOwner',
					ISNULL(u.Username, '') AS 'Username',
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
					LEFT JOIN auth.AspNetUsers u ON ur.Username = u.Username
					LEFT JOIN Prop p ON r.RoomId = p.RoomId
					WHERE u.Username == @userName";
	    
	    var rooms = await connection.QueryAsync<Room, bool, string, Room>(sql, (room, canEdit, user) =>
	    {
		    room.Users.Add(new Room.UserEntry(user, canEdit));
		    return room;
	    }, new { userName = username }, splitOn: "IsOwner, Username, PropId");

	    return MergeRooms(rooms).Select(room => room.ToDto());
    }

    public async Task<RoomRequestDto?> GetRoomByIdAsync(Guid roomId)
    {
	    await using var connection = new SqlConnection(connectionString);
	    const string sql = @"SELECT
					r.RoomId,
					r.[Name],
					r.Width,
                    r.Height,
                    r.TileId,
					ISNULL(ur.IsOwner, 0) AS 'IsOwner',
					ISNULL(u.Username, '') AS 'Username',
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

	    var rooms = await connection.QueryAsync<Room, bool, string, Room>(sql, (room, canEdit, user) =>
	    {
		    if (user == null) return room;
		    room.Users.Add(new Room.UserEntry(user, canEdit));
		    return room;
	    }, new { roomId }, splitOn: "IsOwner, Username, PropId");

	    return MergeRooms(rooms).FirstOrDefault()?.ToDto();
    }

    //todo add user
    public async Task<Guid> AddRoomAsync(RoomCreateDto roomCreateDto)
    {
	    await using var connection = new SqlConnection(connectionString);
	    var id = Guid.NewGuid();
	    await connection.ExecuteAsync("INSERT INTO dbo.[Room] VALUES (RoomId, Name, Width, Height, TileId) = (@roomId, @name, @width, @height, @tileId)",
		    new { roomId = id, name = roomCreateDto.Name, width = roomCreateDto.Width, height = roomCreateDto.Height, tileId = roomCreateDto.TileId });

	    return id;
    }

    public async Task UpdateRoomAsync(Guid roomId, RoomUpdateDto roomUpdateDto)
    {
	    await using var connection = new SqlConnection(connectionString);

	    await connection.ExecuteAsync("UPDATE dbo.[Room] SET Name = @name, Width = @width, Height = @height, TileId = @tileId WHERE RoomId = @roomId", 
		    new { roomId, name = roomUpdateDto.Name, width = roomUpdateDto.Width, height = roomUpdateDto.Height, tileId = roomUpdateDto.TileId });
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
	    await using var connection = new SqlConnection(connectionString);

	    await connection.ExecuteAsync("DELETE FROM dbo.[Room] WHERE RoomId = @roomId", 
		    new { roomId });
    }
    
    private static List<Room> MergeRooms(IEnumerable<Room> rooms)
    {
	    return [.. rooms.GroupBy(room => room.RoomId).Select(group =>
	    {
		    if (!group.Any()) return null;
		    var first = group.First();
		    var room = new Room()
		    {
			    RoomId = first.RoomId,
			    Name = first.Name,
			    Width = first.Width,
			    Height = first.Height,
			    TileId = first.TileId,
		    };

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

		    room.Users = [.. room.Users.DistinctBy(user => user.User)];
		    room.Props = [.. room.Props.DistinctBy(prop => prop.PropId)];

		    return room;
	    }).OfType<Room>()];
    }
}
