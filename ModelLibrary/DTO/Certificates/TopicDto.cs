using ModelLibrary.Models.DTO.Questions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO.Certificates;

public class TopicDto
{
    [Required]
    public int Id { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }

    [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public List<QuestionDto>? Questions { get; set; }
}
