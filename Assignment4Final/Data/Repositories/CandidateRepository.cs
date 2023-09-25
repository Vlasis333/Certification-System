using System.Collections;
using System.Linq.Expressions;
using Assignment4Final.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.Questions;

namespace Assignment4Final.Data.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Candidate>> GetAll()
        {
            if (!CandidatesDbSetExists())
            {
                return Enumerable.Empty<Candidate>();
            }

            return await _context.Candidates
                .Include(a => a.Gender)
                .Include(a => a.Language)
                .Include(a => a.PhotoIdType)
                .Include(a => a.Address)
                .ThenInclude(a => a.Country)
                .ToListAsync();
        }

        public async Task<Candidate?> GetCandidate(string appUserId)
        {
            if (!CandidatesDbSetExists())
            {
                return null;
            }

            return await _context.Candidates
                .Include(a => a.Gender)
                .Include(a => a.Language)
                .Include(a => a.PhotoIdType)
                .Include(a => a.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(p => p.AppUserId == appUserId);
        }

        public async Task<Candidate?> DeleteCandidate(string appUserId)
        {
            // NOTE(vmavraganis): not working needs to delete candidateExams -> we have to make cascade on delete
            var candidate = await _context.Candidates
                .Include(a => a.Gender)
                .Include(a => a.Language)
                .Include(a => a.PhotoIdType)
                .Include(a => a.Address)
                .ThenInclude(a => a.Country)
                .Include(a => a.CandidateExams)
                .FirstOrDefaultAsync(p => p.AppUserId == appUserId);
            if (candidate != null)
            {
                var candidateDeleted = _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();

                return candidateDeleted.Entity;
            }

            return null;
        }

        public async Task<Candidate?> AddCandidate(Candidate candidate)
        {
            if (candidate.Gender != null)
            {
                candidate.Gender = await _context.Genders.FindAsync(candidate.Gender.Id);
            }

            if (candidate.Language != null)
            {
                candidate.Language =
                    await _context.Languages.FindAsync(candidate.Language.Id);
            }

            if (candidate.PhotoIdType != null)
            {
                candidate.PhotoIdType = await _context.PhotoIdTypes.FindAsync(
                    candidate.PhotoIdType.Id
                );
            }

            if (candidate.Address != null)
            {
                if (candidate.Address.Any())
                {
                    foreach (var address in candidate.Address)
                    {
                        var dbAddress = _context.Addresses
                            .Include(a => a.Country)
                            .FirstOrDefault(a => a.Id == address.Id);
                        if (dbAddress != null)
                        {
                            dbAddress.Address1 = address.Address1;
                            dbAddress.Address2 = address.Address2;
                            dbAddress.City = address.City;
                            dbAddress.State = address.State;
                            dbAddress.PostalCode = address.PostalCode;
                            dbAddress.Country = await _context.Countries.FindAsync(
                                address.Country.Id
                            );
                        }
                        else
                        {
                            address.Country = await _context.Countries.FindAsync(
                                address.Country.Id
                            );
                        }
                    }
                }
            }

            var candidateEntry = await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();

            return candidateEntry.Entity;
        }

        public async Task<Candidate?> UpdateCandidate(string id, Candidate candidate)
        {
            var candidateToUpdate = await GetCandidate(id);

            if (candidateToUpdate != null)
            {
                candidateToUpdate.FirstName = candidate.FirstName;
                candidateToUpdate.MiddleName = candidate.MiddleName;
                candidateToUpdate.LastName = candidate.LastName;
                candidateToUpdate.DateOfBirth = candidate.DateOfBirth;
                candidateToUpdate.Email = candidate.Email;
                candidateToUpdate.Landline = candidate.Landline;
                candidateToUpdate.Mobile = candidate.Mobile;
                candidateToUpdate.PhotoIdNumber = candidate.PhotoIdNumber;
                candidateToUpdate.PhotoIdIssueDate = candidate.PhotoIdIssueDate;

                candidateToUpdate.Gender = candidate.Gender;
                candidateToUpdate.Language = candidate.Language;
                candidateToUpdate.PhotoIdType = candidate.PhotoIdType;
                candidateToUpdate.Address = candidate.Address;

                await _context.SaveChangesAsync();
            }

            return candidateToUpdate;
        }

        public async Task<Candidate?> UpdateCandidateAsync(Candidate candidate)
        {
            var dbCandidate = await _context.Candidates
                .Include(c => c.AppUser)
                .Include(c => c.Gender)
                .Include(c => c.Language)
                .Include(c => c.PhotoIdType)
                .Include(c => c.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(c => c.AppUserId == candidate.AppUserId);

            if (dbCandidate != null)
            {
                dbCandidate.FirstName = candidate.FirstName;
                dbCandidate.MiddleName = candidate.MiddleName;
                dbCandidate.LastName = candidate.LastName;
                dbCandidate.DateOfBirth = candidate.DateOfBirth;
                dbCandidate.Email = candidate.Email;
                dbCandidate.Landline = candidate.Landline;
                dbCandidate.Mobile = candidate.Mobile;
                dbCandidate.CandidateNumber = candidate.CandidateNumber;
                dbCandidate.PhotoIdNumber = candidate.PhotoIdNumber;
                dbCandidate.PhotoIdIssueDate = candidate.PhotoIdIssueDate;

                if (candidate.Gender != null)
                {
                    dbCandidate.Gender =
                        await _context.Genders.FindAsync(candidate.Gender.Id);
                }

                if (candidate.Language != null)
                {
                    dbCandidate.Language = await _context.Languages.FindAsync(
                        candidate.Language.Id
                    );
                }

                if (candidate.PhotoIdType != null)
                {
                    dbCandidate.PhotoIdType = await _context.PhotoIdTypes.FindAsync(
                        candidate.PhotoIdType.Id
                    );
                }

                if (candidate.Address != null)
                {
                    // NOTE:(akotro) Delete addresses
                    var dbAddressesToDelete = dbCandidate.Address
                        .Where(a => !candidate.Address.Any(ca => ca.Id == a.Id))
                        .ToList();
                    foreach (var address in dbAddressesToDelete)
                    {
                        // NOTE:(akotro) This seems to not be required,
                        // because it is done magically by entity framework
                        // dbCandidate.Address.Remove(address);
                        _context.Addresses.Remove(address);
                    }

                    if (candidate.Address.Any())
                    {
                        foreach (var address in candidate.Address)
                        {
                            var dbAddress = dbCandidate.Address.FirstOrDefault(
                                a => a.Id == address.Id
                            );
                            if (dbAddress != null)
                            {
                                dbAddress.Address1 = address.Address1;
                                dbAddress.Address2 = address.Address2;
                                dbAddress.City = address.City;
                                dbAddress.State = address.State;
                                dbAddress.PostalCode = address.PostalCode;
                                dbAddress.Country = await _context.Countries.FindAsync(
                                    address.Country.Id
                                );
                            }
                            else
                            {
                                dbCandidate.Address.Add(
                                    new Address
                                    {
                                        Address1 = address.Address1,
                                        Address2 = address.Address2,
                                        City = address.City,
                                        State = address.State,
                                        PostalCode = address.PostalCode,
                                        Country = await _context.Countries.FindAsync(
                                            address.Country.Id
                                        )
                                    }
                                );
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return dbCandidate;
        }

        public bool CandidatesDbSetExists()
        {
            return _context.Candidates != null;
        }
    }
}
