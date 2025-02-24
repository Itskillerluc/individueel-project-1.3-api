using individueel_project_1._3_api.Dto;

namespace individueel_project_1._3_api.Repositories;

public interface IPropRepository
{
    Task<PropRequestDto?> GetPropByIdAsync(Guid propId);
    Task<Guid> AddPropAsync(PropCreateDto prop);
    Task UpdatePropAsync(Guid propId, PropUpdateDto prop);
    Task DeletePropAsync(Guid propId);
}