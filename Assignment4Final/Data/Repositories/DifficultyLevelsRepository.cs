using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Certificates;

namespace Assignment4Final.Data.Repositories;

public class DifficultyLevelsRepository : IDifficultyLevelsRepository
{
    private readonly ApplicationDbContext _context;

    public DifficultyLevelsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DifficultyLevel>> GetAllAsync()
    {
        return await _context.DifficultyLevels.ToListAsync();
        // .Include(dl => dl.Questions)
    }

    public async Task<DifficultyLevel?> GetAsync(int id)
    {
        return await _context.DifficultyLevels.FirstOrDefaultAsync(dl => dl.Id == id);
        // .Include(dl => dl.Questions)
    }

    public async Task<DifficultyLevel?> AddAsync(DifficultyLevel difficultyLevel)
    {
        var difficultyLevelEntry = await _context.DifficultyLevels.AddAsync(difficultyLevel);
        await _context.SaveChangesAsync();

        return difficultyLevelEntry.Entity;
    }

    public async Task<DifficultyLevel?> UpdateAsync(int id, DifficultyLevel difficultyLevel)
    {
        var dbDifficultyLevel = await GetAsync(id);

        if (dbDifficultyLevel != null)
        {
            dbDifficultyLevel.Difficulty = difficultyLevel.Difficulty;
            dbDifficultyLevel.Questions = difficultyLevel.Questions;

            await _context.SaveChangesAsync();
        }

        return dbDifficultyLevel;
    }

    public async Task<DifficultyLevel?> DeleteAsync(int id)
    {
        var difficultyLevel = await _context.DifficultyLevels.FindAsync(id);
        if (difficultyLevel != null)
        {
            var difficultyLevelEntry = _context.DifficultyLevels.Remove(difficultyLevel);
            await _context.SaveChangesAsync();

            return difficultyLevelEntry.Entity;
        }

        return null;
    }

    public bool DifficultyLevelExists(int id)
    {
        return (_context.DifficultyLevels?.Any(dl => dl.Id == id)).GetValueOrDefault();
    }
}
