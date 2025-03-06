using Dapper;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;

namespace individueel_project_1._3_api.Repositories;

public class PropRepository(string connectionString) : IPropRepository
{
    public async Task<PropRequestDto?> GetPropByIdAsync(Guid propId)
    {
        await using var connection = new SqlConnection(connectionString);

        return (await connection.QueryFirstOrDefaultAsync<Prop>("SELECT PropId, PrefabId, PosX, PosY, Rotation, ScaleX, ScaleY, SortingLayer, RoomId FROM dbo.[Prop] WHERE PropId = @propId", 
            new { propId }))?.ToDto();

    }

    public async Task<Guid> AddPropAsync(PropCreateDto prop)
    {
        await using var connection = new SqlConnection(connectionString);
        var id = Guid.NewGuid();
        await connection.ExecuteAsync("INSERT INTO dbo.[Prop] (PropId, PrefabId, PosX, PosY, Rotation, ScaleX, ScaleY, SortingLayer, RoomId) VALUES (@propId, @prefabId, @posX, @posY, @rotation, @scaleX, @scaleY, @sortingLayer, @roomId)",
            new { propId = id , prefabId = prop.PrefabId, posX = prop.PosX, posY = prop.PosY, rotation = prop.Rotation, scaleX = prop.ScaleX, scaleY = prop.ScaleY, sortingLayer = prop.SortingLayer, roomId = prop.RoomId });

        return id;
    }

    public async Task UpdatePropAsync(Guid propId, PropUpdateDto prop)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("UPDATE dbo.[Prop] SET PrefabId = @prefabId, PosX = @posX, PosY = @posY, Rotation = @rotation, ScaleX = @scaleX, ScaleY = @scaleY, SortingLayer = @sortingLayer WHERE PropId = @propId", 
            new { propId, prefabId = prop.PrefabId, posX = prop.PosX, posY = prop.PosY, rotation = prop.Rotation, scaleX = prop.ScaleX, scaleY = prop.ScaleY, sortingLayer = prop.SortingLayer });
    }

    public async Task DeletePropAsync(Guid propId)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("DELETE FROM dbo.[Prop] WHERE PropId = @propId", 
            new { propId });
    }

    public async Task DeletePropsByRoomAsync(Guid roomId)
    {
        await using var connection = new SqlConnection(connectionString);
        
        await connection.ExecuteAsync("DELETE FROM dbo.[Prop] WHERE RoomId = @roomId", 
            new { roomId });
    }
}
