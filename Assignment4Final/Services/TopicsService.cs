using Assignment4Final.Data.Repositories;
using AutoMapper;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Services;

public class TopicsService
{
    private readonly ITopicsRepository _repository;
    private readonly IMapper _mapper;

    public TopicsService(ITopicsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TopicDto>> GetAllAsync()
    {
        var topics = await _repository.GetAllAsync();
        return _mapper.Map<List<TopicDto>>(topics);
    }

    public async Task<TopicDto?> GetAsync(int id)
    {
        var topic = await _repository.GetAsync(id);
        return topic == null ? null : _mapper.Map<TopicDto>(topic);
    }

    public async Task<TopicDto?> AddAsync(TopicDto topicDto)
    {
        var topic = _mapper.Map<Topic>(topicDto);
        var addedTopic = await _repository.AddAsync(topic);
        return addedTopic == null ? null : _mapper.Map<TopicDto>(addedTopic);
    }

    public async Task<TopicDto?> UpdateAsync(int id, TopicDto topicDto)
    {
        var topic = _mapper.Map<Topic>(topicDto);
        var updatedTopic = await _repository.UpdateAsync(id, topic);
        return updatedTopic == null ? null : _mapper.Map<TopicDto>(updatedTopic);
    }

    public async Task<TopicDto?> DeleteAsync(int id)
    {
        var deletedTopic = await _repository.DeleteAsync(id);
        return deletedTopic == null ? null : _mapper.Map<TopicDto>(deletedTopic);
    }

    public bool TopicExists(int id)
    {
        return _repository.TopicExists(id);
    }
}
