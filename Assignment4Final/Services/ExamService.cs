using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.DTO.Exams;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Services
{
    public class ExamService
    {
        private readonly ExamRepository _repository;
        private readonly ITopicsRepository _topicsRepository;
        private readonly IMapper _mapper;

        public ExamService(
            ExamRepository repository,
            ITopicsRepository topicsRepository,
            IMapper mapper
        )
        {
            _repository = repository;
            _topicsRepository = topicsRepository;
            _mapper = mapper;
        }

        public async Task<List<Exam>> GetListOfExamsAsync()
        {
            return await _repository.GetAllExamsAsync();
        }

        public List<ExamDto> GetExamDtoList(List<Exam> exams)
        {
            return _mapper.Map<List<ExamDto>>(exams);
        }

        public Exam GetExamFromExamDto(ExamDto examDto)
        {
            return _mapper.Map<Exam>(examDto);
        }

        public async Task<Exam?> GetExamAsync(int id)
        {
            return await _repository.GetExamAsync(id);
        }

        public async Task<ExamDto?> GetAsync(int id)
        {
            var exam = await _repository.GetExamAsync(id);
            return exam == null ? null : _mapper.Map<ExamDto>(exam);
        }

        public async Task<ExamDto?> AddAsync(ExamDto examDto)
        {
            var exam = _mapper.Map<Exam>(examDto);

            var addedExam = await _repository.AddAsync(exam);
            return addedExam == null ? null : _mapper.Map<ExamDto>(addedExam);
        }

        public async Task<ExamDto?> UpdateAsync(int id, ExamDto examDto)
        {
            var exam = _mapper.Map<Exam>(examDto);
            var updatedExam = await _repository.UpdateAsync(id, exam);
            return updatedExam == null ? null : _mapper.Map<ExamDto>(updatedExam);
        }

        public async Task<ExamDto?> DeleteAsync(int id)
        {
            var deletedExam = await _repository.DeleteAsync(id);
            return deletedExam == null ? null : _mapper.Map<ExamDto>(deletedExam);
        }
    }
}
