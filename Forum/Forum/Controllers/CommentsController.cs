using AutoMapper;
using Forum.Auth.Model;
using Forum.Data.Entities;
using Forum.Data.Repositories;
using Forum.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forum.Controllers
{
    [ApiController]
    [Route("api/topics/{topicId}/posts/{postId}/comment")]
    public class CommentsController : ControllerBase
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ITopicsRepository _topicsRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IAuthorizationService _authorizationService;
        public CommentsController(IPostsRepository postsRepository, IMapper mapper, ITopicsRepository topicsRepository, ICommentsRepository commentsRepository, IAuthorizationService authorizationService)
        {
            _topicsRepository = topicsRepository;
            _mapper = mapper;
            _postsRepository = postsRepository;
            _commentsRepository = commentsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<CommentDTO>> GetAllAsync(int postId)
        {
            var comments = await _commentsRepository.GetAsync(postId);
            return comments.Select(o => _mapper.Map<CommentDTO>(o));
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<CommentDTO>> GetAsync(int postId, int commentId)
        {
            var post = await _commentsRepository.GetAsync(postId, commentId);
            if (post == null) return NotFound();

            return Ok(_mapper.Map<CommentDTO>(post));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<CommentDTO>> PostAsync(int postId, int topicId, CreateCommentDTO commentDto)
        {
            var topic = await _topicsRepository.Get(topicId);
            if (topic == null) return NotFound($"Couldn't find a topic with id of {topicId}");

            var post = await _postsRepository.GetAsync(topicId, postId);
            if (post == null) return NotFound($"Couldn't find a post with id of {topicId}");

            var comment = new Comment
            {
                Description = commentDto.description,
                CreationTimeUtc = DateTime.UtcNow,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            //var comment = _mapper.Map<Comment>(commentDto);
            comment.PostId = postId;

            await _commentsRepository.InsertAsync(comment);
            return Created("", new CreateCommentDTO(comment.Description, comment.CreationTimeUtc));
            //return Created($"/api/topics/{topicId}/posts/{post.Id}/comment/{comment.Id}", _mapper.Map<CommentDTO>(comment));
        }

        [HttpPut("{commentId}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<CommentDTO>> PostAsync(int postId, int topicId, int commentId, CreateCommentDTO commentDto)
        {
            var topic = await _topicsRepository.Get(topicId);
            if (topic == null) return NotFound($"Couldn't find a topic with id of {topicId}");

            var post = await _postsRepository.GetAsync(topicId, postId);
            if (post == null) return NotFound($"Couldn't find a post with id of {topicId}");

            var oldComment = await _commentsRepository.GetAsync(postId, commentId);
            if (oldComment == null) return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, oldComment, PolicyNames.SameUser);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            oldComment.Description = commentDto.description;
            oldComment.CreationTimeUtc = commentDto.CreationTimeUtc;
            //_mapper.Map(commentDto, oldComment);

            await _commentsRepository.UpdateAsync(oldComment);

            return Ok(new CreateCommentDTO(oldComment.Description, oldComment.CreationTimeUtc));
            //return Ok(_mapper.Map<CommentDTO>(oldComment));
        }

        [HttpDelete("{commentId}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult> DeleteAsync(int commentId, int postId)
        {
            var comment = await _commentsRepository.GetAsync(commentId, postId);
            if (comment == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, comment, PolicyNames.SameUser);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _commentsRepository.DeleteAsync(comment);

            // 204
            return NoContent();
        }
    }
}
