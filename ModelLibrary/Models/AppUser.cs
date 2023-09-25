using Microsoft.AspNetCore.Identity;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Exams;

namespace ModelLibrary.Models;

public class AppUser : IdentityUser
{
    public Candidate Candidate { get; set; }
    public Marker Marker { get; set; }
}
