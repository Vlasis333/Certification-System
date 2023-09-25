using ModelLibrary.Models.Candidates;

namespace ModelLibrary.Models.Exams;

public class CandidateExam
{
    public int Id { get; set; }
    public bool? Result { get; set; }
    public int? MaxScore { get; set; }
    public decimal? PercentScore { get; set; }
    public DateTime? ExamDate { get; set; }
    public DateTime? ReportDate { get; set; }
    public int? CandidateScore { get; set; }
    public string? AssessmentCode { get; set; }
    public string? Voucher { get; set; }
    public bool? IsModerated { get; set; }

    public DateTime? MarkerAssignedDate { get; set; }
    public DateTime? MarkingDate { get; set; }

    public virtual Marker? Marker { get; set; }
    public virtual Candidate? Candidate { get; set; }
    public virtual Exam? Exam { get; set; }

    public virtual ICollection<CandidateExamAnswers>? CandidateExamAnswers { get; set; } // NOTE:(akotro) Reverse Navigation
}
