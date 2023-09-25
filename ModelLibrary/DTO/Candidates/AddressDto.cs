using ModelLibrary.Models.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.Candidates
{
    public class AddressDto
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string? Address1 { get; set; }
        [StringLength(100)]
        public string? Address2 { get; set; }
        [Required, StringLength(100)]
        public string? City { get; set; }
        [Required, StringLength(100)]
        public string? State { get; set; }
        // []
        public string? PostalCode { get; set; }
        [Required]
        public CountryDto? Country { get; set; }
    }
}
