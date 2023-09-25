using ModelLibrary.Models.Questions;

namespace ModelLibrary.Models.Certificates;

public enum DifficultyEnum
{
    EASY,
    MEDIUM,
    HARD
}

public class DifficultyLevel
{
    public int Id { get; set; }
    public DifficultyEnum? Difficulty { get; set; }

    public virtual ICollection<Question>? Questions { get; set; } // NOTE:(akotro) Reverse Navigation
}
