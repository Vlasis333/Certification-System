using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Candidates;

namespace Assignment4Final.Data.Repositories;

public class LanguagesRepository : IGenericRepository<Language>
{
    private readonly ApplicationDbContext _context;

    public LanguagesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Language>> GetAllAsync()
    {
        return await _context.Languages.ToListAsync();
    }

    public async Task<Language?> GetAsync(int id)
    {
        return await _context.Languages.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Language?> AddAsync(Language language)
    {
        var languageEntry = await _context.Languages.AddAsync(language);
        await _context.SaveChangesAsync();

        return languageEntry.Entity;
    }

    public async Task<Language?> UpdateAsync(int id, Language language)
    {
        var dbLanguage = await GetAsync(id);

        if (dbLanguage != null)
        {
            dbLanguage.NativeLanguage = language.NativeLanguage;
            dbLanguage.Candidates = language.Candidates;

            await _context.SaveChangesAsync();
        }

        return dbLanguage;
    }

    public async Task<Language?> DeleteAsync(int id)
    {
        var language = await _context.Languages.FindAsync(id);
        if (language != null)
        {
            var languageEntry = _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return languageEntry.Entity;
        }

        return null;
    }

    public bool EntityExists(int id)
    {
        return (_context.Languages?.Any(c => c.Id == id)).GetValueOrDefault();
    }
}
