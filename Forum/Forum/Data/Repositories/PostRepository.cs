using Forum.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public interface IPostsRepository
    {
        Task<Post> GetAsync(int topicId, int postId);
        Task<List<Post>> GetAsync(int topicId);
        Task InsertAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
    }

    public class PostsRepository : IPostsRepository
    {
        private readonly RestContext _restContext;

        public PostsRepository(RestContext RestContext)
        {
            _restContext = RestContext;
        }

        public async Task<Post> GetAsync(int topicId, int postId)
        {
            return await _restContext.Posts.FirstOrDefaultAsync(o => o.TopicId == topicId && o.Id == postId);
        }

        public async Task<List<Post>> GetAsync(int topicId)
        {
            return await _restContext.Posts.Where(o => o.TopicId == topicId).ToListAsync();
        }

        public async Task InsertAsync(Post post)
        {
            _restContext.Posts.Add(post);
            await _restContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _restContext.Posts.Update(post);
            await _restContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _restContext.Posts.Remove(post);
            await _restContext.SaveChangesAsync();
        }
    }
}