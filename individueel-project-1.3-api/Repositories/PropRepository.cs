using Dapper;
using individueel_project_1._3_api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace individueel_project_1._3_api.Repositories;

public class PropRepository(string connectionString) : ICrudRepository<Guid, Prop>
{
    private readonly string _connectionString = connectionString;

    public async Task<bool> AddAsync(Prop entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("INSERT INTO dbo.[Prop] VALUES (PropId, PrefabId, PosX, PosY, Rotation, ScaleX, ScaleY, SortingLayer, RoomId) = (@propId, @prefabId, @posX, @posY, @rotation, @scaleX, @scaleY, @sortingLayer, @roomId)", new { @propId = entity.PropId, prefabId = entity.PrefabId, @posX = entity.PosX, posY = entity.PosY, rotation = entity.Rotation, scaleX = entity.ScaleX, scaleY = entity.ScaleY, sortingLayer = entity.SortingLayer, roomId = entity.RoomId });

        return effected == 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("DELETE FROM dbo.[Prop] WHERE PropId = @propId", new { propId = id });

        return effected == 1;
    }

    public async Task<IEnumerable<Prop>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryAsync<Prop>("SELECT PropId, PrefabId, PosX, PosY, Rotation, ScaleX, ScaleY, SortingLayer, RoomId FROM dbo.[Prop]");
    }

    public async Task<Prop?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<Prop>("SELECT PropId, PrefabId, PosX, PosY, Rotation, ScaleX, ScaleY, SortingLayer, RoomId FROM dbo.[Prop] WHERE PropId = @propId", new { propId = id });
    }

    public async Task<bool> UpdateAsync(Guid id, Prop entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var effected = await connection.ExecuteAsync("UPDATE dbo.[Prop] SET PrefabId = @prefabId, PosX = @posX, PosY = @posY, Rotation = @rotation, ScaleX = @scaleX, ScaleY = @scaleY, SortingLayer = @sortingLayer", new { prefabId = entity.PrefabId, posX = entity.PosX, posY = entity.PosY, rotation = entity.Rotation, scaleX = entity.ScaleX, scaleY = entity.ScaleY, sortingLayer = entity.SortingLayer });

        return effected == 1;
    }
}
