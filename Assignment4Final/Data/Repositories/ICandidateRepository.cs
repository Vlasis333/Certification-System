using System.Linq.Expressions;
using ModelLibrary.Models.Candidates;

namespace Assignment4Final.Data.Repositories
{
    public interface ICandidateRepository
    {
        public Task<IEnumerable<Candidate>> GetAll();
        public Task<Candidate> GetCandidate(string appUserId);
        public Task<Candidate?> DeleteCandidate(string appUserId);
        public bool CandidatesDbSetExists();
        public Task<Candidate?> AddCandidate(Candidate candidate);
        public Task<Candidate?> UpdateCandidate(string id, Candidate candidate);

        public Task<Candidate?> UpdateCandidateAsync(Candidate candidate);
    }
}
