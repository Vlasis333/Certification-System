using Assignment4Final.Data.Repositories;
using AutoMapper;
using Bogus;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.CandidateExam;
using ModelLibrary.Models.DTO.Certificates;
using ModelLibrary.Models.DTO.Exams;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Services
{
    public class CandidateExamService
    {
        private readonly CandidateExamRepository _candidateExamRepository;
        private readonly ExamService _examService;
        private readonly IMapper _mapper;
        private readonly ICertificatesRepository _certificatesRepository;

        public CandidateExamService(
            CandidateExamRepository candidateExamRepository,
            IMapper mapper,
            ExamService examService,
            ICertificatesRepository certificatesRepository
        )
        {
            _candidateExamRepository = candidateExamRepository;
            _mapper = mapper;
            _examService = examService;
            _certificatesRepository = certificatesRepository;
        }

        public async Task<Candidate?> GetCandidateByUserIdAsync(string userId)
        {
            return await _candidateExamRepository.GetCandidateByUserIdAsync(userId);
        }

        //public async Task<CandidateExam> GetCandidateExamByExam(Exam exam, string userId)  //NOT nedded maybe
        //{
        //    var candidate = await GetCandidateByUserIdAsync(userId);
        //    var examEntity = await _examService.GetExamAsync(exam.Id);
        //    var candidateExam = new CandidateExam()
        //    {
        //        Candidate = candidate,
        //        Exam = examEntity,
        //        Voucher = Guid.NewGuid().ToString()
        //    };
        //    return candidateExam;
        //}




        public async Task<CandidateExamDto?> GetCandidateExamByCertificateAsync(
            int certId,
            string userId
        )
        {
            var certificate = await _certificatesRepository.GetAsync(certId); // used this so the entity is loaded
            if (!certificate.Exams.ToList().Any())
            {
                return null;
            }
            var exam = GetRandomExam(certificate.Exams.ToList());

            var faker = new Faker();

            var candExam = new CandidateExam
            {
                Candidate = await _candidateExamRepository.GetCandidateByUserIdAsync(userId),
                Exam = exam,
                Voucher = faker.Random.AlphaNumeric(10)
            };

            var candExamUpdated = await _candidateExamRepository.AddOrUpdateAsync(candExam);

            return _mapper.Map<CandidateExamDto>(candExamUpdated);
        }

        public Exam GetRandomExam(List<Exam> exams)
        {
            Random random = new Random();
            int count = exams.Count();
            var exam = exams[random.Next(0, count)];
            while (exam == null)
            {
                exam = exams[random.Next(0, count)];
            }
            return exam;
        }

        public async Task<CandidateExam> AddCandidateExamAsync(CandidateExam candidateExam)
        {
            return candidateExam = await _candidateExamRepository.AddOrUpdateAsync(candidateExam);
        }

        public CandidateExamDto GetCandidateExamDtoFromCandidateExam(CandidateExam candidateExam)
        {
            _candidateExamRepository.LoadCertificateOfCandidateExamEntity(ref candidateExam);
            return _mapper.Map<CandidateExamDto>(candidateExam);
        }

        public async Task<List<CandidateExam>> GetAllCandidateExamsOfCandidateAsync(
            Candidate candidate
        )
        {
            return await _candidateExamRepository.GetAllCandidateExamsOfCandidateAsync(candidate);
        }

        public async Task<List<CandidateExam>> GetNotTakenCandidateExamsOfCandidateAsync(
            Candidate candidate
        )
        {
            return await _candidateExamRepository.GetNotTakenCandidateExamsOfCandidateAsync(
                candidate
            );
        }

        public List<CandidateExamDto> GetListOfCandidateExamDtosFromListOfCandidateExam(
            List<CandidateExam> candidateExams
        )
        {
            return _mapper.Map<List<CandidateExamDto>>(candidateExams);
        }

        public async Task<CandidateExam?> GetCandidateExamByIdAsync(int id)
        {
            return await _candidateExamRepository.GetCandidateExamByIdAsync(id);
        }

        public async Task<CandidateExamDto?> UpdateWithAnswersCandidateExamDtoAsync(
            CandidateExam candidateExam
        )
        {
            if (candidateExam != null)
            {
                var candidateExamUpdated = await CreateCandidateExamAnswersAsync(candidateExam);
                var mapped = _mapper.Map<CandidateExamDto>(candidateExamUpdated);
                return mapped;
            }
            return null;
        }

        public async Task<CandidateExam?> CreateCandidateExamAnswersAsync(
            CandidateExam candidateExam
        )
        {
            if (
                candidateExam.Exam != null
                && candidateExam.Exam.Questions != null
                && candidateExam.CandidateExamAnswers != null
            )
            {
                candidateExam.ExamDate = DateTime.Now;
                var questions = candidateExam.Exam.Questions.ToList();

                questions.ForEach(
                    question =>
                        candidateExam.CandidateExamAnswers.Add(
                            new CandidateExamAnswers()
                            {
                                Question = question,
                                QuestionText = question.Text,
                                CorrectOption =
                                    question.Options != null
                                        ? question.Options
                                            .Where(opt => opt.Correct)
                                            .FirstOrDefault()
                                            .Text
                                        : null,
                            }
                        )
                );
                var candidateExamUpdated = await Task.Run(
                    () => _candidateExamRepository.AddOrUpdateAsync(candidateExam)
                );
                return candidateExamUpdated;
            }
            return null;
        }

        public CandidateExamDto GetCandidateExamDto(CandidateExam candidateExam)
        {
            return _mapper.Map<CandidateExamDto>(candidateExam);
        }

        public async Task<CandidateExam> UpdatdeWithResults(CandidateExam candidateExam)
        {
            //var score = candidateExam.CandidateExamAnswers.Count(answer => (bool)answer.IsCorrect); // IsCorrect must not be nullable
            var score = candidateExam.CandidateExamAnswers.Count(
                answer => (answer.IsCorrect != null && (bool)answer.IsCorrect)
            );

            candidateExam.MaxScore = candidateExam.Exam.MaxMark;

            candidateExam.CandidateScore = score;
            candidateExam.ReportDate = DateTime.Now;

            decimal precScore = ((decimal)score / (decimal)candidateExam.MaxScore) * (decimal)100;
            candidateExam.PercentScore = decimal.Round(precScore, 2); // max score shoud be on exam not candidateExam
            candidateExam.Result =
                candidateExam.CandidateScore >= candidateExam.Exam?.PassMark ? true : false;
            return await _candidateExamRepository.AddOrUpdateAsync(candidateExam);
        }
    }
}
