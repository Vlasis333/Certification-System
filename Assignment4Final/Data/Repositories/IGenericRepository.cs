namespace Assignment4Final.Data.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync() => throw new NotImplementedException();
    Task<TEntity?> GetAsync(int id) => throw new NotImplementedException();
    Task<TEntity?> GetAsync(string id) => throw new NotImplementedException();
    Task<TEntity?> AddAsync(TEntity entity) => throw new NotImplementedException();
    Task<TEntity?> UpdateAsync(int id, TEntity entity) => throw new NotImplementedException();
    Task<TEntity?> UpdateAsync(string id, TEntity entity) => throw new NotImplementedException();
    Task<TEntity?> DeleteAsync(int id) => throw new NotImplementedException();
    Task<TEntity?> DeleteAsync(string id) => throw new NotImplementedException();
    bool EntityExists(int id) => throw new NotImplementedException();
    bool EntityExists(string id) => throw new NotImplementedException();
}
