using System.Configuration;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.Questions;

namespace ModelLibrary.Models.Exams;

public class Exam
{
    public int Id { get; set; }
    public int PassMark { get; set; }
    public int MaxMark { get; set; }

    public virtual Certificate? Certificate { get; set; }
    public virtual ICollection<Question>? Questions { get; set; }

    public virtual ICollection<CandidateExam>? CandidateExams { get; set; } // NOTE:(akotro) Reverse Navigation
}
