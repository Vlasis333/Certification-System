using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO.Questions;

public class OptionDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public bool Correct { get; set; }
}
