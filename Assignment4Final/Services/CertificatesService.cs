using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Services;

public class CertificatesService
{
    private readonly ICertificatesRepository _repository;
    private readonly IMapper _mapper;

    public CertificatesService(ICertificatesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CertificateDto>> GetAllAsync()
    {
        var certificates = await _repository.GetAllAsync();
        return _mapper.Map<List<CertificateDto>>(certificates);
    }

    public async Task<CertificateDto?> GetAsync(int id, bool include = true)
    {
        var certificate = await _repository.GetAsync(id, include);
        return certificate == null ? null : _mapper.Map<CertificateDto>(certificate);
    }

    public async Task<CertificateDto?> AddAsync(CertificateDto certificateDto)
    {
        var certificate = _mapper.Map<Certificate>(certificateDto);
        var addedCertificate = await _repository.AddAsync(certificate);
        return addedCertificate == null ? null : _mapper.Map<CertificateDto>(addedCertificate);
    }

    public async Task<CertificateDto?> UpdateAsync(int id, CertificateDto certificateDto)
    {
        var certificate = _mapper.Map<Certificate>(certificateDto);
        var updatedCertificate = await _repository.UpdateAsync(id, certificate);
        return updatedCertificate == null ? null : _mapper.Map<CertificateDto>(updatedCertificate);
    }

    public async Task<CertificateDto?> DeleteAsync(int id)
    {
        var deletedCertificate = await _repository.DeleteAsync(id);
        return deletedCertificate == null ? null : _mapper.Map<CertificateDto>(deletedCertificate);
    }

    public bool CertificateExists(int id)
    {
        return _repository.CertificateExists(id);
    }
}
