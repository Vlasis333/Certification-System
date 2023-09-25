using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Services;

public class CountriesService
{
    private readonly IGenericRepository<Country> _repository;
    private readonly IMapper _mapper;

    public CountriesService(IGenericRepository<Country> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CountryDto>> GetAllAsync()
    {
        var countries = await _repository.GetAllAsync();
        return _mapper.Map<List<CountryDto>>(countries);
    }

    public async Task<CountryDto?> GetAsync(int id)
    {
        var country = await _repository.GetAsync(id);
        return country == null ? null : _mapper.Map<CountryDto>(country);
    }

    public async Task<CountryDto?> AddAsync(CountryDto countryDto)
    {
        var country = _mapper.Map<Country>(countryDto);
        var addedCountry = await _repository.AddAsync(country);
        return addedCountry == null ? null : _mapper.Map<CountryDto>(addedCountry);
    }

    public async Task<CountryDto?> UpdateAsync(int id, CountryDto countryDto)
    {
        var country = _mapper.Map<Country>(countryDto);
        var updatedCountry = await _repository.UpdateAsync(id, country);
        return updatedCountry == null ? null : _mapper.Map<CountryDto>(updatedCountry);
    }

    public async Task<CountryDto?> DeleteAsync(int id)
    {
        var deletedCountry = await _repository.DeleteAsync(id);
        return deletedCountry == null ? null : _mapper.Map<CountryDto>(deletedCountry);
    }

    public bool CountryExists(int id)
    {
        return _repository.EntityExists(id);
    }
}
