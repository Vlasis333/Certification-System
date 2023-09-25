using ModelLibrary.Models.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.Candidates
{
    public class GenderDto
    {
        [Required]
        public int Id { get; set; }

        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum? GenderType { get; set; }
    }
}
