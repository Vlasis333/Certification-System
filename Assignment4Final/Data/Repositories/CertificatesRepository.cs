using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Certificates;

namespace Assignment4Final.Data.Repositories;

public class CertificatesRepository : ICertificatesRepository
{
    private readonly ApplicationDbContext _context;

    public CertificatesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Certificate>> GetAllAsync()
    {
        return await _context.Certificates
            .AsSplitQuery()
            .Include(c => c.Topics)
            .Include(c => c.Exams)
            .ToListAsync();
    }

    public async Task<Certificate?> GetAsync(int id, bool include = true)
    {
        if (!include)
        {
            return await _context.Certificates.FirstOrDefaultAsync(q => q.Id == id);
        }

        return await _context.Certificates
            .AsSplitQuery()
            .Include(q => q.Topics)
            .ThenInclude(topic => topic.Questions)
            .Include(q => q.Exams)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Certificate?> AddAsync(Certificate certificate)
    {
        // TODO:(akotro) Does this need to be changed here??
        await FillNavigationProperties(certificate);

        var certificateEntry = await _context.Certificates.AddAsync(certificate);
        await _context.SaveChangesAsync();

        return certificateEntry.Entity;
    }

    public async Task<Certificate?> UpdateAsync(int id, Certificate certificate)
    {
        var dbCertificate = await GetAsync(id);

        if (dbCertificate != null)
        {
            dbCertificate.Title = certificate.Title;
            dbCertificate.Description = certificate.Description;
            dbCertificate.Category = certificate.Category;
            dbCertificate.Active = certificate.Active;
            dbCertificate.Price = certificate.Price;

            if (certificate.Topics != null)
            {
                var dbTopicsToDelete = dbCertificate.Topics
                    .Where(t => !certificate.Topics.Any(ct => ct.Id == t.Id))
                    .ToList();
                foreach (var topic in dbTopicsToDelete)
                {
                    // NOTE:(akotro) Do not actually delete Topic, just remove from candidate
                    // _context.Topics.Remove(topic);
                    dbCertificate.Topics.Remove(topic);
                }

                if (certificate.Topics.Any())
                {
                    foreach (var topic in certificate.Topics)
                    {
                        // var dbTopic = dbCertificate.Topics.FirstOrDefault(t => t.Id == topic.Id);
                        var dbTopic = await _context.Topics.FirstOrDefaultAsync(
                            t => t.Id == topic.Id
                        );
                        if (dbTopic != null)
                        {
                            if (dbCertificate.Topics.Any(t => t.Id == dbTopic.Id))
                            {
                                // NOTE:(akotro) If certificate already has this topic, update it
                                dbTopic.Name = topic.Name;
                            }
                            else
                            {
                                // NOTE:(akotro) If certificate does not already have the topic, add it
                                dbCertificate.Topics.Add(dbTopic);
                            }
                        }
                        else
                        {
                            // NOTE:(akotro) If the topic does not exist, create it
                            dbCertificate.Topics.Add(new Topic { Name = topic.Name });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        return dbCertificate;
    }

    public async Task<Certificate?> DeleteAsync(int id)
    {
        var certificate = await _context.Certificates
            .Include(c => c.Exams)
            .ThenInclude(e => e.CandidateExams)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (certificate != null)
        {
            // TODO:(akotro) Do we want to actually delete the topics and exams as well?
            if (certificate.Exams != null)
            {
                certificate.Exams
                    .ToList()
                    .ForEach(e => e.CandidateExams.ToList().ForEach(ce => ce.Exam = null));
            }

            var certificateEntry = _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return certificateEntry.Entity;
        }

        return null;
    }

    public bool CertificateExists(int id)
    {
        return (_context.Certificates?.Any(c => c.Id == id)).GetValueOrDefault();
    }

    private async Task FillNavigationProperties(Certificate certificate)
    {
        certificate.Topics = certificate.Topics
            ?.Select(async t => await _context.Topics.FindAsync(t.Id))
            .Select(t => t.Result)
            .ToList();
        certificate.Exams = certificate.Exams
            ?.Select(async e => await _context.Exams.FindAsync(e.Id))
            .Select(e => e.Result)
            .ToList();
    }
}
