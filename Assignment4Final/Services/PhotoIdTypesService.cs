using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Services;

public class PhotoIdTypesService
{
    private readonly IGenericRepository<PhotoIdType> _repository;
    private readonly IMapper _mapper;

    public PhotoIdTypesService(IGenericRepository<PhotoIdType> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PhotoIdTypeDto>> GetAllAsync()
    {
        var photoIdTypes = await _repository.GetAllAsync();
        return _mapper.Map<List<PhotoIdTypeDto>>(photoIdTypes);
    }

    public async Task<PhotoIdTypeDto?> GetAsync(int id)
    {
        var photoIdType = await _repository.GetAsync(id);
        return photoIdType == null ? null : _mapper.Map<PhotoIdTypeDto>(photoIdType);
    }

    public async Task<PhotoIdTypeDto?> AddAsync(PhotoIdTypeDto photoIdTypeDto)
    {
        var photoIdType = _mapper.Map<PhotoIdType>(photoIdTypeDto);
        var addedPhotoIdType = await _repository.AddAsync(photoIdType);
        return addedPhotoIdType == null ? null : _mapper.Map<PhotoIdTypeDto>(addedPhotoIdType);
    }

    public async Task<PhotoIdTypeDto?> UpdateAsync(int id, PhotoIdTypeDto photoIdTypeDto)
    {
        var photoIdType = _mapper.Map<PhotoIdType>(photoIdTypeDto);
        var updatedPhotoIdType = await _repository.UpdateAsync(id, photoIdType);
        return updatedPhotoIdType == null ? null : _mapper.Map<PhotoIdTypeDto>(updatedPhotoIdType);
    }

    public async Task<PhotoIdTypeDto?> DeleteAsync(int id)
    {
        var deletedPhotoIdType = await _repository.DeleteAsync(id);
        return deletedPhotoIdType == null ? null : _mapper.Map<PhotoIdTypeDto>(deletedPhotoIdType);
    }

    public bool PhotoIdTypeExists(int id)
    {
        return _repository.EntityExists(id);
    }
}
