using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Candidates;

namespace Assignment4Final.Data.Repositories;

public class CountriesRepository : IGenericRepository<Country>
{
    private readonly ApplicationDbContext _context;

    public CountriesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Country>> GetAllAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetAsync(int id)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country?> AddAsync(Country country)
    {
        var countryEntry = await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();

        return countryEntry.Entity;
    }

    public async Task<Country?> UpdateAsync(int id, Country country)
    {
        var dbCountry = await GetAsync(id);

        if (dbCountry != null)
        {
            dbCountry.CountryOfResidence = country.CountryOfResidence;
            dbCountry.Addresses = country.Addresses;

            await _context.SaveChangesAsync();
        }

        return dbCountry;
    }

    public async Task<Country?> DeleteAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country != null)
        {
            var countryEntry = _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return countryEntry.Entity;
        }

        return null;
    }

    public bool EntityExists(int id)
    {
        return (_context.Countries?.Any(c => c.Id == id)).GetValueOrDefault();
    }
}
