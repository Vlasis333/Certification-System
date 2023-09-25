using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models.Questions;
using ModelLibrary.Models.DTO.Questions;
using AutoMapper;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Data.Repositories;

public class QuestionsRepository : IQuestionsRepository
{
    // TODO:(akotro) This should be refactored to use only entities
    // and dto operations should be moved to QuestionsService

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QuestionsRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Question>> GetAllAsync()
    {
        return await _context.Questions
            .Include(q => q.DifficultyLevel)
            .Include(q => q.Topic)
            .Include(q => q.Options)
            .ToListAsync();
    }

    public async Task<Question?> GetAsync(int? id, bool include = true)
    {
        if (!include)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        }

        return await _context.Questions
            .Include(q => q.DifficultyLevel)
            .Include(q => q.Topic)
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Question?> AddAsync(QuestionDto questionDto)
    {
        var question = new Question
        {
            Text = questionDto.Text,
            Topic =
                questionDto.Topic != null
                    ? _mapper.Map<Topic>(questionDto.Topic)
                    : await _context.Topics.FindAsync(questionDto.TopicId),
            DifficultyLevel =
                questionDto.DifficultyLevel != null
                    ? _mapper.Map<DifficultyLevel>(questionDto.DifficultyLevel)
                    : await _context.DifficultyLevels.FindAsync(questionDto.DifficultyLevelId),
            Options = questionDto.Options
                ?.Select(o => new Option { Text = o.Text, Correct = o.Correct })
                .ToList()
        };

        var questionEntry = await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();

        return questionEntry.Entity;
    }

    public async Task<Question?> UpdateAsync(int id, QuestionDto questionDto)
    {
        var question = await GetAsync(id);

        if (question != null)
        {
            question.Text = questionDto.Text;
            if (questionDto.Topic != null)
            {
                question.Topic = await _context.Topics.FindAsync(questionDto.Topic.Id);
            }
            else
            {
                question.Topic = await _context.Topics.FindAsync(questionDto.TopicId);
            }
            if (questionDto.DifficultyLevel != null)
            {
                question.DifficultyLevel = await _context.DifficultyLevels.FindAsync(
                    questionDto.DifficultyLevel.Id
                );
            }
            else
            {
                question.DifficultyLevel = await _context.DifficultyLevels.FindAsync(
                    questionDto.DifficultyLevelId
                );
            }

            if (questionDto.Options != null)
            {
                var optionsToDelete = question.Options
                    .Where(o => !questionDto.Options.Any(odto => odto.Id == o.Id))
                    .ToList();
                foreach (var option in optionsToDelete)
                {
                    _context.Remove(option);
                }

                foreach (var optionDto in questionDto.Options)
                {
                    var option = question.Options.FirstOrDefault(o => o.Id == optionDto.Id);
                    if (option != null)
                    {
                        option.Text = optionDto.Text;
                        option.Correct = optionDto.Correct;
                    }
                    else
                    {
                        question.Options.Add(
                            new Option
                            {
                                Text = optionDto.Text,
                                Correct = optionDto.Correct,
                                Question = question
                            }
                        );
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        return question;
    }

    public async Task<Question?> Delete(int id)
    {
        // var question = await _context.Questions.FindAsync(id);
        var question = await _context.Questions
            .Include(question => question.Options)
            .Include(question => question.CandidateExamAnswers)
            .Where(question => question.Id == id)
            .FirstOrDefaultAsync();

        if (question != null)
        {
            if (question.Options != null)
            {
                foreach (var option in question.Options)
                {
                    _context.Options.Remove(option);
                }
            }

            var questionEntry = _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return questionEntry.Entity;
        }

        return null;
    }

    public bool QuestionsDbSetExists()
    {
        return _context.Questions != null;
    }

    public bool QuestionExists(int id)
    {
        return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public IQueryable<SelectListItem> GetTopicsSelectList()
    {
        return _context.Topics.Select(
            t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }
        );
    }

    public IQueryable<SelectListItem> GetDifficultyLevelsSelectList()
    {
        return _context.DifficultyLevels.Select(
            d => new SelectListItem { Text = d.Difficulty.ToString(), Value = d.Id.ToString() }
        );
    }

    public QuestionDto CreateDto()
    {
        return new QuestionDto()
        {
            Options = new List<OptionDto>()
            {
                new OptionDto(),
                new OptionDto(),
                new OptionDto(),
                new OptionDto()
            }
        };
    }

    public async Task<QuestionDto> CreateDto(int id)
    {
        var question = await _context.Questions
            .Include(q => q.Options)
            .Include(q => q.Topic)
            .Include(q => q.DifficultyLevel)
            .FirstOrDefaultAsync(q => q.Id == id);

        return new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            TopicId = question.Topic.Id,
            DifficultyLevelId = question.DifficultyLevel.Id,
            Options = question.Options
                .Select(
                    o =>
                        new OptionDto
                        {
                            Id = o.Id,
                            Text = o.Text,
                            Correct = o.Correct
                        }
                )
                .ToList()
        };
    }
}
