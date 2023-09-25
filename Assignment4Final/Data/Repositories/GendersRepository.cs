using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Candidates;

namespace Assignment4Final.Data.Repositories;

public class GendersRepository : IGenericRepository<Gender>
{
    private readonly ApplicationDbContext _context;

    public GendersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Gender>> GetAllAsync()
    {
        return await _context.Genders.ToListAsync();
    }

    public async Task<Gender?> GetAsync(int id)
    {
        return await _context.Genders.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Gender?> AddAsync(Gender gender)
    {
        var genderEntry = await _context.Genders.AddAsync(gender);
        await _context.SaveChangesAsync();

        return genderEntry.Entity;
    }

    public async Task<Gender?> UpdateAsync(int id, Gender gender)
    {
        var dbGender = await GetAsync(id);

        if (dbGender != null)
        {
            dbGender.GenderType = gender.GenderType;

            await _context.SaveChangesAsync();
        }

        return dbGender;
    }

    public async Task<Gender?> DeleteAsync(int id)
    {
        var gender = await _context.Genders.FindAsync(id);
        if (gender != null)
        {
            var genderEntry = _context.Genders.Remove(gender);
            await _context.SaveChangesAsync();

            return genderEntry.Entity;
        }

        return null;
    }

    public bool EntityExists(int id)
    {
        return (_context.Genders?.Any(g => g.Id == id)).GetValueOrDefault();
    }
}
