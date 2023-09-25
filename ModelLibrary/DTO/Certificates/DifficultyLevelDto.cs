using System.ComponentModel.DataAnnotations;
using ModelLibrary.Models.Certificates;

namespace ModelLibrary.Models.DTO.Certificates;

public class DifficultyLevelDto
{
    [Required]
    public int Id { get; set; }

    [EnumDataType(typeof(DifficultyEnum))]
    public DifficultyEnum? Difficulty { get; set; }
}
