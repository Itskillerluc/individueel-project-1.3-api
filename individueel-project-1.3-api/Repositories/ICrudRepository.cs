namespace individueel_project_1._3_api.Repositories;

public interface ICrudRepository<K, V>
{
    Task<IEnumerable<V>> GetAllAsync();
    Task<V?> GetByIdAsync(K id);
    Task<bool> AddAsync(V entity);
    Task<bool> UpdateAsync(K id, V updated);
    Task<bool> DeleteAsync(K id);
}
