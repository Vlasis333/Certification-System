using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Services;

public class LanguagesService
{
    private readonly IGenericRepository<Language> _repository;
    private readonly IMapper _mapper;

    public LanguagesService(IGenericRepository<Language> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<LanguageDto>> GetAllAsync()
    {
        var languages = await _repository.GetAllAsync();
        return _mapper.Map<List<LanguageDto>>(languages);
    }

    public async Task<LanguageDto?> GetAsync(int id)
    {
        var language = await _repository.GetAsync(id);
        return language == null ? null : _mapper.Map<LanguageDto>(language);
    }

    public async Task<LanguageDto?> AddAsync(LanguageDto languageDto)
    {
        var language = _mapper.Map<Language>(languageDto);
        var addedLanguage = await _repository.AddAsync(language);
        return addedLanguage == null ? null : _mapper.Map<LanguageDto>(addedLanguage);
    }

    public async Task<LanguageDto?> UpdateAsync(int id, LanguageDto languageDto)
    {
        var language = _mapper.Map<Language>(languageDto);
        var updatedLanguage = await _repository.UpdateAsync(id, language);
        return updatedLanguage == null ? null : _mapper.Map<LanguageDto>(updatedLanguage);
    }

    public async Task<LanguageDto?> DeleteAsync(int id)
    {
        var deletedLanguage = await _repository.DeleteAsync(id);
        return deletedLanguage == null ? null : _mapper.Map<LanguageDto>(deletedLanguage);
    }

    public bool LanguageExists(int id)
    {
        return _repository.EntityExists(id);
    }
}
