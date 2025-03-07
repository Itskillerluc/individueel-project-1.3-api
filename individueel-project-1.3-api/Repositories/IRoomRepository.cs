﻿using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<RoomRequestDto>> GetRoomsByUserAsync(string username);
    Task<RoomRequestDto?> GetRoomByIdAsync(Guid roomId, string username);
    Task<RoomRequestDto?> GetRoomByNameAndUserAsync(string roomName, string username);
    Task<RoomRequestDto?> GetRoomByPropAsync(Guid propId, string username);

    Task<Guid> AddRoomAsync(RoomCreateDto roomCreateDto);
    
    Task UpdateRoomAsync(Guid roomId, RoomUpdateDto roomUpdateDto);

    Task DeleteRoomAsync(Guid roomId);
}