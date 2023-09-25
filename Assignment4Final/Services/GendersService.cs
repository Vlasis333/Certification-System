using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Services;

public class GendersService
{
    private readonly IGenericRepository<Gender> _repository;
    private readonly IMapper _mapper;

    public GendersService(IGenericRepository<Gender> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GenderDto>> GetAllAsync()
    {
        var genders = await _repository.GetAllAsync();
        return _mapper.Map<List<GenderDto>>(genders);
    }

    public async Task<GenderDto?> GetAsync(int id)
    {
        var gender = await _repository.GetAsync(id);
        return gender == null ? null : _mapper.Map<GenderDto>(gender);
    }

    public async Task<GenderDto?> AddAsync(GenderDto genderDto)
    {
        var gender = _mapper.Map<Gender>(genderDto);
        var addedGender = await _repository.AddAsync(gender);
        return addedGender == null ? null : _mapper.Map<GenderDto>(addedGender);
    }

    public async Task<GenderDto?> UpdateAsync(int id, GenderDto genderDto)
    {
        var gender = _mapper.Map<Gender>(genderDto);
        var updatedGender = await _repository.UpdateAsync(id, gender);
        return updatedGender == null ? null : _mapper.Map<GenderDto>(updatedGender);
    }

    public async Task<GenderDto?> DeleteAsync(int id)
    {
        var deletedGender = await _repository.DeleteAsync(id);
        return deletedGender == null ? null : _mapper.Map<GenderDto>(deletedGender);
    }

    public bool GenderExists(int id)
    {
        return _repository.EntityExists(id);
    }
}
