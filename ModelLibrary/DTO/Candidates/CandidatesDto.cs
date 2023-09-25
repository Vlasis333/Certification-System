using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Exams;
using ModelLibrary.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.Candidates
{
    public class CandidatesDto
    {
        [Required]
        public string AppUserId { get; set; }

        [Required,
        StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }

        [Required, Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? DateOfBirth { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Landline { get; set; }

        [Phone]
        public string? Mobile { get; set; }

        [StringLength(50)]
        public string? CandidateNumber { get; set; }

        [Required, StringLength(50)]
        public string? PhotoIdNumber { get; set; }

        [Required, Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? PhotoIdIssueDate { get; set; }

        public PhotoIdTypeDto? PhotoIdType { get; set; }
        public GenderDto? Gender { get; set; }
        public LanguageDto? Language { get; set; }
        public ICollection<AddressDto>? Address { get; set; }
    }
}
