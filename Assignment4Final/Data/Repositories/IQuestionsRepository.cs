using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.Questions;

namespace Assignment4Final.Data.Repositories;

public interface IQuestionsRepository
{
    Task<List<Question>> GetAllAsync();
    Task<Question?> GetAsync(int? id, bool include = true);
    Task<Question?> AddAsync(QuestionDto questionDto);
    Task<Question?> UpdateAsync(int id, QuestionDto questionDto);
    Task<Question?> Delete(int id);
    public bool QuestionsDbSetExists();
    bool QuestionExists(int id);
    IQueryable<SelectListItem> GetTopicsSelectList();
    IQueryable<SelectListItem> GetDifficultyLevelsSelectList();
    QuestionDto CreateDto();
    Task<QuestionDto> CreateDto(int id);
}