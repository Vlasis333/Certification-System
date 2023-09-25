using ModelLibrary.Models.Certificates;

namespace Assignment4Final.Data.Repositories;

public interface ITopicsRepository
{
    Task<List<Topic>> GetAllAsync();
    Task<Topic?> GetAsync(int id);
    Task<Topic?> AddAsync(Topic topic);
    Task<Topic?> UpdateAsync(int id, Topic topic);
    Task<Topic?> DeleteAsync(int id);
    bool TopicExists(int id);
}