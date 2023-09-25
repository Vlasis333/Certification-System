using ModelLibrary.Models.DTO.Exams;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO.Certificates;

public class CertificateDto
{
    [Required]
    public int Id { get; set; }
    public double? Price { get; set; }
    [Required, StringLength(100)]
    public string? Title { get; set; }
    [StringLength(1000)]
    public string? Description { get; set; }
    [StringLength(300)]
    public string? Category { get; set; }
    [Required]
    public bool? Active { get; set; }

    [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public List<TopicDto>? Topics { get; set; }

    [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public List<ExamDto>? Exams { get; set; }
}
