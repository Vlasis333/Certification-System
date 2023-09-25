using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.DTO.CandidateExam;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Services;

public class CandidateExamAnswersService
{
    private readonly IGenericRepository<CandidateExamAnswers> _repository;
    private readonly IMapper _mapper;

    public CandidateExamAnswersService(
        IGenericRepository<CandidateExamAnswers> repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CandidateExamAnswersDto>> GetAllAsync()
    {
        var candidateExamAnswers = await _repository.GetAllAsync();
        return _mapper.Map<List<CandidateExamAnswersDto>>(candidateExamAnswers);
    }

    public async Task<CandidateExamAnswersDto?> GetAsync(int id)
    {
        var candidateExamAnswer = await _repository.GetAsync(id);
        return candidateExamAnswer == null
            ? null
            : _mapper.Map<CandidateExamAnswersDto>(candidateExamAnswer);
    }

    public async Task<CandidateExamAnswersDto?> AddAsync(
        CandidateExamAnswersDto candidateExamAnswerDto
    )
    {
        var candidateExamAnswer = _mapper.Map<CandidateExamAnswers>(candidateExamAnswerDto);
        var addedCandidateExamAnswer = await _repository.AddAsync(candidateExamAnswer);
        return addedCandidateExamAnswer == null
            ? null
            : _mapper.Map<CandidateExamAnswersDto>(addedCandidateExamAnswer);
    }

    public async Task<CandidateExamAnswersDto?> UpdateAsync(
        int id,
        CandidateExamAnswersDto candidateExamAnswerDto
    )
    {
        var candidateExamAnswer = _mapper.Map<CandidateExamAnswers>(candidateExamAnswerDto);
        var updatedCandidateExamAnswer = await _repository.UpdateAsync(id, candidateExamAnswer);
        return updatedCandidateExamAnswer == null
            ? null
            : _mapper.Map<CandidateExamAnswersDto>(updatedCandidateExamAnswer);
    }

    public async Task<CandidateExamAnswersDto?> DeleteAsync(int id)
    {
        var deletedCandidateExamAnswer = await _repository.DeleteAsync(id);
        return deletedCandidateExamAnswer == null
            ? null
            : _mapper.Map<CandidateExamAnswersDto>(deletedCandidateExamAnswer);
    }

    public bool CandidateExamAnswerExists(int id)
    {
        return _repository.EntityExists(id);
    }
}
