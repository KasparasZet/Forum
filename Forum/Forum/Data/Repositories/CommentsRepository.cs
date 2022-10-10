using Forum.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
    public interface ICommentsRepository
    {
        Task DeleteAsync(Comment comment);
        Task<List<Comment>> GetAsync(int postId);
        Task<Comment> GetAsync(int postId, int commentId);
        Task InsertAsync(Comment comment);
        Task UpdateAsync(Comment comment);
    }

    public class CommentsRepository : ICommentsRepository
    {
        private readonly RestContext _restContext;
        public CommentsRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Comment> GetAsync(int postId, int commentId)
        {
            return await _restContext.Comments.FirstOrDefaultAsync(o => o.PostId == postId && o.Id == commentId);
        }

        public async Task<List<Comment>> GetAsync(int postId)
        {
            return await _restContext.Comments.Where(o => o.PostId == postId).ToListAsync();
        }

        public async Task InsertAsync(Comment comment)
        {
            _restContext.Comments.Add(comment);
            await _restContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _restContext.Comments.Update(comment);
            await _restContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _restContext.Comments.Remove(comment);
            await _restContext.SaveChangesAsync();
        }
    }
}
