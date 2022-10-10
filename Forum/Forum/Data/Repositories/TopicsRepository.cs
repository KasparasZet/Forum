using Forum.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
    public interface ITopicsRepository
    {
        Task<List<Topic>> GetAll();
        Task<Topic> Get(int id);
        Task Create(Topic topic);
        Task Put(Topic topic);
        Task Delete(Topic topic);
    }

    public class TopicsRepository : ITopicsRepository
    {
        private readonly RestContext _restContext;

        public TopicsRepository(RestContext RestContext)
        {
            _restContext = RestContext;
        }

        public async Task<List<Topic>> GetAll()
        {
            return await _restContext.Topics.ToListAsync();
        }

        public async Task<Topic> Get(int id)
        {
            return await _restContext.Topics.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task Create(Topic topic)
        {
            _restContext.Topics.Add(topic);
            await _restContext.SaveChangesAsync();
        }

        public async Task Put(Topic topic)
        {
            _restContext.Topics.Update(topic);
            await _restContext.SaveChangesAsync();
        }

        public async Task Delete(Topic topic)
        {
            _restContext.Topics.Remove(topic);
            await _restContext.SaveChangesAsync();
        }
    }
}
