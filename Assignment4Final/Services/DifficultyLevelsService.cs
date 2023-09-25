using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Services;

public class DifficultyLevelsService
{
    private readonly IDifficultyLevelsRepository _repository;
    private readonly IMapper _mapper;

    public DifficultyLevelsService(IDifficultyLevelsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<DifficultyLevelDto>> GetAllAsync()
    {
        var difficultyLevels = await _repository.GetAllAsync();
        return _mapper.Map<List<DifficultyLevelDto>>(difficultyLevels);
    }

    public async Task<DifficultyLevelDto?> GetAsync(int id)
    {
        var difficultyLevel = await _repository.GetAsync(id);
        return difficultyLevel == null
            ? null
            : _mapper.Map<DifficultyLevelDto>(difficultyLevel);
    }

    public async Task<DifficultyLevelDto?> AddAsync(DifficultyLevelDto difficultyLevelDto)
    {
        var difficultyLevel = _mapper.Map<DifficultyLevel>(difficultyLevelDto);
        var addedDifficultyLevel = await _repository.AddAsync(difficultyLevel);
        return addedDifficultyLevel == null
            ? null
            : _mapper.Map<DifficultyLevelDto>(addedDifficultyLevel);
    }

    public async Task<DifficultyLevelDto?> UpdateAsync(
        int id,
        DifficultyLevelDto difficultyLevelDto
    )
    {
        var difficultyLevel = _mapper.Map<DifficultyLevel>(difficultyLevelDto);
        var updatedDifficultyLevel = await _repository.UpdateAsync(id, difficultyLevel);
        return updatedDifficultyLevel == null
            ? null
            : _mapper.Map<DifficultyLevelDto>(updatedDifficultyLevel);
    }

    public async Task<DifficultyLevelDto?> DeleteAsync(int id)
    {
        var deletedDifficultyLevel = await _repository.DeleteAsync(id);
        return deletedDifficultyLevel == null
            ? null
            : _mapper.Map<DifficultyLevelDto>(deletedDifficultyLevel);
    }

    public bool DifficultyLevelExists(int id)
    {
        return _repository.DifficultyLevelExists(id);
    }
}
