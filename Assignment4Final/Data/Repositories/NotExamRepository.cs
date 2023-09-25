using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Exams;
using ModelLibrary.Models.Questions;
using System;

namespace Assignment4Final.Data.Repositories
{
    public class NotExamRepository : IExamRepository
    {
        private ApplicationDbContext _context;

        //public ExamRepository()
        //{
        //    _context = new ApplicationDbContext();
        //}

        public NotExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// vmavraganis: Async Task to get all the questions with their options related tot he current exam
        /// </summary>
        public async Task<IEnumerable<Question>> GetAllQuestionsAsync(
            CandidateExam candidateExam)
        {
            var exam = await Task.Run(() => _context.Exams
                .Include(p => p.Questions)
                .ThenInclude(p => p.Options)
                .Where(p => p == candidateExam.Exam).SingleOrDefault());

            if (exam != null)
            {
                if (exam.Questions != null)
                {
                    return exam.Questions;
                }
                else
                {
                    // Note(vmavraganis): case no exam is found we return an empty collection so we can handle it later
                    return Enumerable.Empty<Question>();
                }
            }
            else
            {
                return Enumerable.Empty<Question>();
            }
        }

        /// <summary>
        /// vmavraganis: Async Task to add the new examAnswers and update the candidateExam
        /// </summary>
        public async Task<CandidateExam> UpdateCandidateExam(
            IEnumerable<string> dropDownOptions, CandidateExam candidateExam)
        {
            _context.Entry(candidateExam).Reference(c => c.Exam).Load();
            var examQuestions = candidateExam.Exam;
            _context.Entry(examQuestions).Collection(c => c.Questions).Load();

            candidateExam.MaxScore = candidateExam.Exam.Questions.Count;
            candidateExam.ReportDate = DateAndTime.Now;

            _context.CandidateExams.Update(candidateExam);

            int candScore = AddCandidateExamAnswers(dropDownOptions,
                candidateExam.Exam.Questions, candidateExam);

            candidateExam.CandidateScore = candScore;
            candidateExam.PercentScore =
                CalculatePercentageScore(candidateExam.Exam.Questions.Count, candScore);
            //Note (vmavraganis): passingMark (65) is to be changed dynamicaly
            candidateExam.Result =
                Passed(candidateExam.Exam.Questions.Count, candScore, 65);

            await _context.SaveChangesAsync();
            return candidateExam;
        }

        /// <summary>
        /// vmavraganis: Calculates the percentage score based on the maxScore and the candidateScore
        /// </summary>
        /// <returns>The percentage score of the candidate</returns>
        public decimal CalculatePercentageScore(int maxScore, int candidateScore)
        {
            return (candidateScore / maxScore) * 100;
        }

        /// <summary>
        /// vmavraganis: Calculates the passingMark % of both scores (candidate and max)
        /// </summary>
        /// <returns>The passed results for the candidate (bool)</returns>
        public bool Passed(int maxScore, int candidateScore, double passingMark)
        {
            passingMark = (passingMark / 100);
            //Note (vmavraganis): calculate's the percentage based on the passing mark (assumes this is the 100%) and then checks for the candidate's mark
            int percentageMaxScore = (int)(maxScore / passingMark);
            int percentageCandidateScore = (int)(candidateScore / passingMark);

            if (percentageCandidateScore >= percentageMaxScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// vmavraganis: Calculates the final score of the candidate and adds the exam answers entities
        /// </summary>
        /// <returns>The final score of the candidate (score)</returns>
        public int AddCandidateExamAnswers(IEnumerable<string> dropDownOptions,
            IEnumerable<Question> questions, CandidateExam candidateExam)
        {
            int index = 0;
            int candidateScore = 0;
            foreach (var question in questions)
            {
                _context.Entry(question).Collection(c => c.Options).Load();
                //Note (vmavraganis): we use hasValue only when the correct is nullable, after it we can just regular expresion with Where
                //string? correct = question.Options.FirstOrDefault(option => option.Correct).Text;
                string? correct = question.Options
                    .FirstOrDefault(option => option.Correct).Text;
                string? choosen = question.Options
                    .ElementAt(int.Parse(dropDownOptions.ElementAt(index)) - 1).Text;
                //Note (vmavraganis): dropDownOptions.ElementAt(index)) - 1 == dropDown hold 1 to 4 so we subtract 1 to get the corresponding entry at options (0 to 3)
                bool? isCorrect = question.Options
                    .ElementAt(int.Parse(dropDownOptions.ElementAt(index)) - 1).Correct;
                //Note (vmavraganis): remove nullable if model is changed

                if (isCorrect == true)
                {
                    candidateScore++;
                }

                var examAnswers = new CandidateExamAnswers
                {
                    CorrectOption = correct,
                    ChosenOption = choosen,
                    IsCorrect = isCorrect,
                    CandidateExam = candidateExam
                };
                _context.CandidateExamAnswers.Add(examAnswers);
                index++;
            }

            return candidateScore;
        }

        private bool _dispose = false;

        /// <summary>
        /// vmavraganis: Displose method and private field to destroy object when not needed
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            if (!this._dispose)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this._dispose = true;
        }

        void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
