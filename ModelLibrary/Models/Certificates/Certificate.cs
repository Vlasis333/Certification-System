using ModelLibrary.Models.Exams;

namespace ModelLibrary.Models.Certificates;

public class Certificate
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool? Active { get; set; }
    public double? Price { get; set; }
    public virtual ICollection<Topic>? Topics { get; set; }

    public virtual ICollection<Exam>?
        Exams { get; set; } // NOTE:(akotro) Reverse Navigation
}
