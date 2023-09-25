using ModelLibrary.Models.Questions;

namespace ModelLibrary.Models.Certificates;

public class Topic
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public virtual ICollection<Certificate>? Certificates { get; set; }

    public virtual ICollection<Question>?
        Questions { get; set; } // NOTE:(akotro) Reverse Navigation
}
