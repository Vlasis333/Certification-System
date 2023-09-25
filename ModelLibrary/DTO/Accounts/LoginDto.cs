using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ModelLibrary.Models.DTO.Accounts;

public class LoginDto
{
    // public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    // [Required]
    public string? Password { get; set; }
    public bool? IsCandidate { get; set; }
}
