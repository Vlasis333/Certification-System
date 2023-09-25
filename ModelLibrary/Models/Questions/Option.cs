namespace ModelLibrary.Models.Questions;

public class Option
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public bool Correct { get; set; }
    public virtual Question? Question { get; set; }
}
