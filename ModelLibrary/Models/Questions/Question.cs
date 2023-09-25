using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.Exams;

namespace ModelLibrary.Models.Questions;

public class Question
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public virtual Topic? Topic { get; set; }
    public virtual DifficultyLevel? DifficultyLevel { get; set; }
    public virtual ICollection<Exam>? Exams { get; set; }

    public virtual ICollection<Option>? Options { get; set; } // NOTE:(akotro) Reverse Navigation
    public virtual ICollection<CandidateExamAnswers>? CandidateExamAnswers { get; set; } // NOTE:(akotro) Reverse Navigation
}
