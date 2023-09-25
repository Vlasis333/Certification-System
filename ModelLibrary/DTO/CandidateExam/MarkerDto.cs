using ModelLibrary.Models.DTO.Accounts;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO.CandidateExam;

public class MarkerDto
{
    [Required]
    public string AppUserId { get; set; }
    public UserDto? AppUser { get; set; }

    [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public List<CandidateExamDto>? CandidateExams { get; set; }
}
