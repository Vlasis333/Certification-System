namespace ModelLibrary.Models.Candidates
{
    public class Address
    {
        public int Id { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public virtual Country? Country { get; set; }
        public virtual Candidate? Candidate { get; set; } // NOTE:(akotro) Reverse Navigation
    }
}
