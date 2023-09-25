namespace ModelLibrary.Models.Exams;

public class Marker
{
    public string AppUserId { get; set; } // NOTE:(akotro) PK + FK to AppUser
    public AppUser AppUser { get; set; }

    public virtual ICollection<CandidateExam>?
        CandidateExams { get; set; } // NOTE:(akotro) Reverse navigation
}
