using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Exams;
using NuGet.Versioning;

namespace Assignment4Final.Data.Repositories
{
    public class CandidateExamRepository : ICandidateExamRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate?> GetCandidateByUserIdAsync(string userId)
        {
            return await _context.Candidates
                .Where(cand => cand.AppUser.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<CandidateExam> AddOrUpdateAsync(CandidateExam candidateExam)
        {
            CandidateExam candidateExamResult = null;
            if (candidateExam.Id != 0)
            {
                candidateExamResult = _context.CandidateExams.Update(candidateExam).Entity;
                await _context.SaveChangesAsync();
                return candidateExamResult;
            }
            candidateExamResult = _context.CandidateExams.Add(candidateExam).Entity;
            _context.SaveChanges();
            return candidateExamResult;
        }

        public void LoadCertificateOfCandidateExamEntity(ref CandidateExam candidateExam)
        {
            _context.Entry(candidateExam).Reference("Exam").Load();
            if (candidateExam.Exam != null)
            {
                _context.Entry(candidateExam.Exam).Reference("Certificate").Load();
            }
        }

        public async Task<List<CandidateExam>> GetAllCandidateExamsOfCandidateAsync(
            Candidate candidate
        )
        {
            return await _context.CandidateExams
                .Include(candExam => candExam.Exam)
                .ThenInclude(exam => exam.Certificate)
                .Include(canExam => canExam.Candidate)
                .Where(candExam => candExam.Candidate == candidate)
                .ToListAsync();
        }

        public async Task<List<CandidateExam>> GetNotTakenCandidateExamsOfCandidateAsync(
            Candidate candidate
        )
        {
            return await _context.CandidateExams
                .Include(candExam => candExam.Candidate)
                .Include(candExam => candExam.Exam)
                .ThenInclude(exam => exam.Certificate)
                .Where(candExam => (candExam.Candidate == candidate && candExam.Result == null))
                .ToListAsync();
        }

        public async Task<CandidateExam?> GetCandidateExamByIdAsync(int id)
        {
            return await _context.CandidateExams
                .AsSplitQuery()
                .Include(candExam => candExam.Candidate)
                .Include(candExam => candExam.CandidateExamAnswers)
                .Include(candExam => candExam.Exam)
                .ThenInclude(cert => cert.Certificate)
                .Include(candExam => candExam.Exam)
                .ThenInclude(exam => exam.Questions)
                .ThenInclude(question => question.Options)
                .Where(candExam => candExam.Id == id)
                .FirstOrDefaultAsync();
        }

        //public void LoadCandidateExam(ref CandidateExam candidateExam)
        // {

        //     _context.Entry(candidateExam)
        //       .Collection(e => e.CandidateExams)
        //         .Query()
        //         .Include(ce => ce.Candidate)
        //       .Load();
        //     _context.Entry(exam)
        //       .Collection(e => e.Questions)
        //       .Load();
        // }
    }

    public interface ICandidateExamRepository
    {
    }
}
