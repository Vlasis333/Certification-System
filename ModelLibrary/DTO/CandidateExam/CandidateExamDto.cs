using ModelLibrary.Models.DTO.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary.Models.DTO.Exams;
using ModelLibrary.Models.Exams;
using Newtonsoft.Json;
using ModelLibrary.Models.DTO.Candidates;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO.CandidateExam
{
    public class CandidateExamDto
    {
        [Required]
        public int Id { get; set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public ExamDto? Exam { get; set; }
        public bool? Result { get; set; }
        public int? MaxScore { get; set; }
        public decimal? PercentScore { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? ExamDate { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? ReportDate { get; set; }
        public int? CandidateScore { get; set; }

        [StringLength(20)]
        public string? AssessmentCode { get; set; }

        [StringLength(20)]
        public string? Voucher { get; set; }
        public bool? IsModerated { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? MarkerAssignedDate { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public DateTime? MarkingDate { get; set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public CandidatesDto? Candidate { get; set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public MarkerDto? Marker { get; set; }

        [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public List<CandidateExamAnswersDto>? CandidateExamAnswers { get; set; }
    }
}
