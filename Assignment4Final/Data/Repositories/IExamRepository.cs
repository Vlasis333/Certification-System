using ModelLibrary.Models.Exams;
using ModelLibrary.Models.Questions;

namespace Assignment4Final.Data.Repositories
{
    public interface IExamRepository
    {
        public Task<IEnumerable<Question>> GetAllQuestionsAsync(
            CandidateExam candidateExam);

        public Task<CandidateExam> UpdateCandidateExam(
            IEnumerable<string> dropDownOptions, CandidateExam candidateExam);

        public bool Passed(int maxScore, int candidateScore, double passingMark);
        public decimal CalculatePercentageScore(int maxScore, int candidateScore);

        public int AddCandidateExamAnswers(IEnumerable<string> dropDownOptions,
            IEnumerable<Question> questions, CandidateExam candidateExam);
    }
}