using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Data.Repositories;

public class CandidateExamAnswersRepository : IGenericRepository<CandidateExamAnswers>
{
    private readonly ApplicationDbContext _context;

    public CandidateExamAnswersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CandidateExamAnswers>> GetAllAsync()
    {
        return await _context.CandidateExamAnswers
            .Include(a => a.CandidateExam)
            .ThenInclude(ce => ce.Exam)
            .ThenInclude(e => e.Certificate)
            .ToListAsync();
    }

    public async Task<CandidateExamAnswers?> GetAsync(int id)
    {
        return await _context.CandidateExamAnswers
            .Include(a => a.Question)
            .Include(a => a.CandidateExam)
            .ThenInclude(ce => ce.Exam)
            .ThenInclude(e => e.Certificate)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CandidateExamAnswers?> AddAsync(CandidateExamAnswers candidateExamAnswer)
    {
        var candidateExamAnswerEntry = await _context.CandidateExamAnswers.AddAsync(
            candidateExamAnswer
        );
        await _context.SaveChangesAsync();

        return candidateExamAnswerEntry.Entity;
    }

    public async Task<CandidateExamAnswers?> UpdateAsync(
        int id,
        CandidateExamAnswers candidateExamAnswer
    )
    {
        var dbCandidateExamAnswer = await GetAsync(id);

        if (dbCandidateExamAnswer != null)
        {
            dbCandidateExamAnswer.QuestionText = candidateExamAnswer.QuestionText;
            dbCandidateExamAnswer.CorrectOption = candidateExamAnswer.CorrectOption;
            dbCandidateExamAnswer.ChosenOption = candidateExamAnswer.ChosenOption;

            if (
                String.Equals(
                    dbCandidateExamAnswer.ChosenOption,
                    dbCandidateExamAnswer.CorrectOption
                )
            )
            {
                dbCandidateExamAnswer.IsCorrect = true;
            }
            else
            {
                dbCandidateExamAnswer.IsCorrect = false;
            }

            dbCandidateExamAnswer.IsCorrectModerated = candidateExamAnswer.IsCorrectModerated;
            // dbCandidateExamAnswer.CandidateExam = candidateExamAnswer.CandidateExam;

            await _context.SaveChangesAsync();
        }

        return dbCandidateExamAnswer;
    }

    public async Task<CandidateExamAnswers?> DeleteAsync(int id)
    {
        var candidateExamAnswer = await _context.CandidateExamAnswers.FindAsync(id);
        if (candidateExamAnswer != null)
        {
            var candidateExamAnswerEntry = _context.CandidateExamAnswers.Remove(
                candidateExamAnswer
            );
            await _context.SaveChangesAsync();

            return candidateExamAnswerEntry.Entity;
        }

        return null;
    }

    public bool EntityExists(int id)
    {
        return (_context.CandidateExamAnswers?.Any(c => c.Id == id)).GetValueOrDefault();
    }
}
