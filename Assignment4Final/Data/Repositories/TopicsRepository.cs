using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Certificates;

namespace Assignment4Final.Data.Repositories;

public class TopicsRepository : ITopicsRepository
{
    private readonly ApplicationDbContext _context;

    public TopicsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Topic>> GetAllAsync()
    {
        return await _context.Topics
            .AsSplitQuery()
            .Include(t => t.Certificates)
            .Include(t => t.Questions)
            .ToListAsync();
    }

    public async Task<Topic?> GetAsync(int id)
    {
        return await _context.Topics
            .AsSplitQuery()
            .Include(t => t.Certificates)
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Topic?> AddAsync(Topic topic)
    {
        await FillNavigationProperties(topic);

        var topicEntry = await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();

        return topicEntry.Entity;
    }

    public async Task<Topic?> UpdateAsync(int id, Topic topic)
    {
        var dbTopic = await GetAsync(id);

        if (dbTopic != null)
        {
            dbTopic.Name = topic.Name;
            dbTopic.Certificates = topic.Certificates;
            dbTopic.Questions = topic.Questions;

            await FillNavigationProperties(dbTopic);

            await _context.SaveChangesAsync();
        }

        return dbTopic;
    }

    public async Task<Topic?> DeleteAsync(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            var topicEntry = _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topicEntry.Entity;
        }

        return null;
    }

    public bool TopicExists(int id)
    {
        return (_context.Topics?.Any(t => t.Id == id)).GetValueOrDefault();
    }

    private async Task FillNavigationProperties(Topic topic)
    {
        topic.Certificates = topic.Certificates
            ?.Select(async c => await _context.Certificates.FindAsync(c.Id))
            .Select(c => c.Result)
            .ToList();
        topic.Questions = topic.Questions
            ?.Select(async q => await _context.Questions.FindAsync(q.Id))
            .Select(q => q.Result)
            .ToList();
    }
}
