using ModelLibrary.Models.DTO.Questions;
using Newtonsoft.Json;

namespace ModelLibrary.Models.DTO.CandidateExam;

public class CandidateExamAnswersDto
{
    // [Required]
    public int Id { get; set; }
    public string? QuestionText { get; set; }
    public string? CorrectOption { get; set; }
    public string? ChosenOption { get; set; }
    public bool? IsCorrect { get; set; }
    public bool? IsCorrectModerated { get; set; }

    [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public QuestionDto? Question { get; set; }

    [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public CandidateExamDto? CandidateExam { get; set; }
}
