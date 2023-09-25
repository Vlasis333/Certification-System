using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary.Models.DTO.Certificates;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.Questions;
using Newtonsoft.Json;

namespace ModelLibrary.Models.DTO.Exams
{
    public class ExamDto
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        public string? CertificateTitle { get; set; }
        public int? PassMark { get; set; }
        public int? MaxMark { get; set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public CertificateDto? Certificate { get; set; }

        [JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public List<QuestionDto>? Questions { get; set; }
    }
}
