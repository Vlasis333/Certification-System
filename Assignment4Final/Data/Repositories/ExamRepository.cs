using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Data.Repositories
{
    public class ExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Exam>> GetAllExamsAsync(bool include = true)
        {
            if (include)
            {
                return await _context.Exams
                    .AsSplitQuery()
                    .Include(exam => exam.Questions)
                    .ThenInclude(q => q.Topic)
                    .Include(exam => exam.Questions)
                    .ThenInclude(quest => quest.DifficultyLevel)
                    .Include(exam => exam.Certificate)
                    .ThenInclude(c => c.Topics)
                    .ToListAsync();
            }
            return await _context.Exams.ToListAsync();
        }

        public async Task<Exam?> GetExamAsync(int id)
        {
            return await _context.Exams
                .AsSplitQuery()
                .Include(exam=> exam.CandidateExams)
                .Include(exam => exam.Questions)
                .Include(exam => exam.Certificate)
                .ThenInclude(c => c.Topics)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.DifficultyLevel)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // TODO:(akotro) Business logic in service to choose random questions?
        public async Task<Exam?> AddAsync(Exam exam)
        {
            await FillNavigationProperties(exam);

            var examEntry = await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();

            return examEntry.Entity;
        }

        public async Task<Exam?> UpdateAsync(int id, Exam exam)
        {
            var dbExam = await GetExamAsync(id);

            if (dbExam != null)
            {
                dbExam.PassMark = exam.PassMark;
                dbExam.MaxMark = exam.MaxMark;

                if (exam.Questions != null)
                {
                    var dbQuestionsToDelete = dbExam.Questions
                        .Where(dbq => !exam.Questions.Any(eq => eq.Id == dbq.Id))
                        .ToList();
                    foreach (var dbQuestion in dbQuestionsToDelete)
                    {
                        // NOTE:(akotro) Remove question from exam
                        dbExam.Questions.Remove(dbQuestion);
                    }

                    if (exam.Questions.Any())
                    {
                        foreach (var question in exam.Questions)
                        {
                            var dbQuestion = await _context.Questions.FirstOrDefaultAsync(
                                q => q.Id == question.Id
                            );
                            if (dbQuestion != null)
                            {
                                if (!dbExam.Questions.Any(q => q.Id == dbQuestion.Id))
                                {
                                    // NOTE:(akotro) If exam does not already have the question, add it
                                    dbExam.Questions.Add(dbQuestion);
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return dbExam;
        }

        public async Task<Exam?> DeleteAsync(int id)
        {
            var exam = await GetExamAsync(id);

            if (exam != null)
            {
                var examEntry = _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();

                return examEntry.Entity;
            }

            return null;
        }

        private async Task FillNavigationProperties(Exam exam)
        {
            exam.Certificate = await _context.Certificates.FindAsync(exam.Certificate?.Id);
            exam.Questions = exam.Questions
                ?.Select(async q => await _context.Questions.FindAsync(q.Id))
                .Select(t => t.Result)
                .ToList();
        }
    }
}
