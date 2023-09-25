using Assignment4Final.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.Questions;

namespace Assignment4Final.Services;

public class QuestionsService
{
    private readonly IQuestionsRepository _repository;

    public QuestionsService(IQuestionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Question>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Question?> GetAsync(int? id, bool include = true)
    {
        return await _repository.GetAsync(id, include);
    }

    public async Task<Question?> AddAsync(QuestionDto questionDto)
    {
        return await _repository.AddAsync(questionDto);
    }

    public async Task<Question?> UpdateAsync(int id, QuestionDto questionDto)
    {
        return await _repository.UpdateAsync(id, questionDto);
    }

    public async Task<Question?> Delete(int id)
    {
        return await _repository.Delete(id);
    }

    public bool QuestionsDbSetExists()
    {
        return _repository.QuestionsDbSetExists();
    }

    public bool QuestionExists(int id)
    {
        return _repository.QuestionExists(id);
    }

    public IQueryable<SelectListItem> GetTopicsSelectList()
    {
        return _repository.GetTopicsSelectList();
    }

    public IQueryable<SelectListItem> GetDifficultyLevelsSelectList()
    {
        return _repository.GetDifficultyLevelsSelectList();
    }

    public QuestionDto CreateDto()
    {
        return _repository.CreateDto();
    }

    public async Task<QuestionDto> CreateDto(int id)
    {
        return await _repository.CreateDto(id);
    }
}
