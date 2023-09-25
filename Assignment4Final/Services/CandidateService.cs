using Assignment4Final.Data.Repositories;
using AutoMapper;
using Bogus;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Candidates;
using ModelLibrary.Models.DTO.Certificates;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.Questions;
using NuGet.Protocol.Core.Types;

namespace Assignment4Final.Services
{
    public class CandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMapper _mapper;

        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper)
        {
            _candidateRepository = candidateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CandidatesDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<CandidatesDto>>(await _candidateRepository.GetAll());
        }

        public async Task<CandidatesDto> GetCandidateById(string appUserId)
        {
            return _mapper.Map<CandidatesDto>(await _candidateRepository.GetCandidate(appUserId));
        }

        public async Task<CandidatesDto?> DeleteCandidate(string appUserId)
        {
            return _mapper.Map<CandidatesDto>(
                await _candidateRepository.DeleteCandidate(appUserId)
            );
        }

        public async Task<CandidatesDto?> AddCandidate(CandidatesDto candidateDto)
        {
            var faker = new Faker();
            candidateDto.CandidateNumber = faker.Random.Number(100000000, 999999999).ToString();

            var addedCandidate = await _candidateRepository.AddCandidate(
                _mapper.Map<Candidate>(candidateDto)
            );
            return addedCandidate != null ? _mapper.Map<CandidatesDto>(addedCandidate) : null;
        }

        public async Task<CandidatesDto?> UpdateCandidate(string id, CandidatesDto candidateDto)
        {
            // var updatedCandidate = await _candidateRepository.AddOrUpdateEntity<Candidate>(
            //     _mapper.Map<Candidate>(candidateDto),
            //     c => c.Address,
            //     c => c.Gender,
            //     c => c.Language,
            //     c => c.PhotoIdType
            // );
            // var updatedCandidate = await _candidateRepository.UpdateCandidate(
            //     id,
            //     _mapper.Map<Candidate>(candidateDto)
            // );
            var updatedCandidate = await _candidateRepository.UpdateCandidateAsync(
                _mapper.Map<Candidate>(candidateDto)
            );
            return updatedCandidate != null ? _mapper.Map<CandidatesDto>(updatedCandidate) : null;
        }
    }
}
