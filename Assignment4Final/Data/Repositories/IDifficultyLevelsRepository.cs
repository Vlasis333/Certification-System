using ModelLibrary.Models.Certificates;

namespace Assignment4Final.Data.Repositories;

public interface IDifficultyLevelsRepository
{
    Task<List<DifficultyLevel>> GetAllAsync();
    Task<DifficultyLevel?> GetAsync(int id);
    Task<DifficultyLevel?> AddAsync(DifficultyLevel difficultyLevel);

    Task<DifficultyLevel?> UpdateAsync(int id,
        DifficultyLevel difficultyLevel);

    Task<DifficultyLevel?> DeleteAsync(int id);
    bool DifficultyLevelExists(int id);
}