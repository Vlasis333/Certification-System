using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Candidates;

namespace Assignment4Final.Data.Repositories;

public class PhotoIdTypesRepository : IGenericRepository<PhotoIdType>
{
    private readonly ApplicationDbContext _context;

    public PhotoIdTypesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PhotoIdType>> GetAllAsync()
    {
        return await _context.PhotoIdTypes.ToListAsync();
    }

    public async Task<PhotoIdType?> GetAsync(int id)
    {
        return await _context.PhotoIdTypes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PhotoIdType?> AddAsync(PhotoIdType photoIdType)
    {
        var photoIdTypeEntry = await _context.PhotoIdTypes.AddAsync(photoIdType);
        await _context.SaveChangesAsync();

        return photoIdTypeEntry.Entity;
    }

    public async Task<PhotoIdType?> UpdateAsync(int id, PhotoIdType photoIdType)
    {
        var dbPhotoIdType = await GetAsync(id);

        if (dbPhotoIdType != null)
        {
            dbPhotoIdType.IdType = photoIdType.IdType;
            dbPhotoIdType.Candidates = photoIdType.Candidates;

            await _context.SaveChangesAsync();
        }

        return dbPhotoIdType;
    }

    public async Task<PhotoIdType?> DeleteAsync(int id)
    {
        var photoIdType = await _context.PhotoIdTypes.FindAsync(id);
        if (photoIdType != null)
        {
            var photoIdTypeEntry = _context.PhotoIdTypes.Remove(photoIdType);
            await _context.SaveChangesAsync();

            return photoIdTypeEntry.Entity;
        }

        return null;
    }

    public bool EntityExists(int id)
    {
        return (_context.PhotoIdTypes?.Any(c => c.Id == id)).GetValueOrDefault();
    }
}
