﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.Candidates
{
    public class CountryDto
    {
        [Required]
        public int Id { get; set; }
        [StringLength(20)]
        public string? CountryOfResidence { get; set; }
    }
}
