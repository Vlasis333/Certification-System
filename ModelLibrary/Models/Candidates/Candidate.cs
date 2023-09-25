using System.ComponentModel.DataAnnotations.Schema;
using ModelLibrary.Models.Exams;

namespace ModelLibrary.Models.Candidates
{
    public class Candidate
    {
        public string AppUserId { get; set; } // NOTE:(akotro) PK + FK to AppUser
        public AppUser AppUser { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Landline { get; set; }
        public string? Mobile { get; set; }
        public string? CandidateNumber { get; set; }
        public string? PhotoIdNumber { get; set; }
        public DateTime? PhotoIdIssueDate { get; set; }

        public virtual Gender? Gender { get; set; }
        public virtual Language? Language { get; set; }
        public virtual PhotoIdType? PhotoIdType { get; set; }

        public virtual ICollection<Address>? Address { get; set; }

        public virtual ICollection<CandidateExam>? CandidateExams { get; set; } // NOTE:(akotro) Reverse Navigation
    }
}
