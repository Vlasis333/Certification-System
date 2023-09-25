using ModelLibrary.Models.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.Candidates
{
    public class PhotoIdTypeDto
    {
        [Required]
        public int Id { get; set; }

        [EnumDataType(typeof(PhotoIdTypeEnum))]
        public PhotoIdTypeEnum? IdType { get; set; }
    }
}
