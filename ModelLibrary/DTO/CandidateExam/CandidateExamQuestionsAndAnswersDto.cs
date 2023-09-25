using ModelLibrary.Models.DTO.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.DTO.CandidateExam
{
    public class CandidateExamQuestionsAndAnswersDto
    {
        public List<QuestionDto>? QuestionsDtos { get; set; }

        public List<CandidateExamAnswersDto>? AnswersDtos { get; set; }
    }
}
