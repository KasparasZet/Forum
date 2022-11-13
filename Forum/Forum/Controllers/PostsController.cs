using AutoMapper;
using Forum.Auth.Model;
using Forum.Data.Entities;
using Forum.Data.Repositories;
using Forum.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    [ApiController]
    [Route("api/topics/{topicId}/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ITopicsRepository _topicsRepository;
        private readonly IAuthorizationService _authorizationService;

        public PostsController(IPostsRepository postsRepository, IMapper mapper, ITopicsRepository topicsRepository, IAuthorizationService authorizationService)
        {
            _postsRepository = postsRepository;
            _mapper = mapper;
            _topicsRepository = topicsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<PostDTO>> GetAllAsync(int topicId)
        {
            var topics = await _postsRepository.GetAsync(topicId);
            return topics.Select(o => _mapper.Map<PostDTO>(o));
        }

        // /api/topics/1/posts/2
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostDTO>> GetAsync(int topicId, int postId)
        {
            var topic = await _postsRepository.GetAsync(topicId, postId);
            if (topic == null) return NotFound();

            return Ok(_mapper.Map<PostDTO>(topic));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<PostDTO>> PostAsync(int topicId, CreatePostDTO postDto)
        {
            var topic = await _topicsRepository.Get(topicId);
            if (topic == null) return NotFound($"Couldn't find a topic with id of {topicId}");

            var post = new Post
            {
                Name = postDto.name,
                Description = postDto.description,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            //var post = _mapper.Map<Post>(postDto);
            post.TopicId = topicId;

            await _postsRepository.InsertAsync(post);

            return Created("", new PostDTO(post.Id, post.Name, post.Description));
            //return Created($"/api/topics/{topicId}/posts/{post.Id}", _mapper.Map<PostDTO>(post));
        }

        [HttpPut("{postId}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<PostDTO>> PostAsync(int topicId, int postId, UpdatePostDTO postDto)
        {
            var topic = await _topicsRepository.Get(topicId);
            if (topic == null) return NotFound($"Couldn't find a topic with id of {topicId}");


            var oldPost = await _postsRepository.GetAsync(topicId, postId);
            if (oldPost == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, oldPost, PolicyNames.SameUser);

            if(!authorizationResult.Succeeded)
            {
                return Forbid();
            }


            oldPost.Name = postDto.name;
            oldPost.Description = postDto.description;
            //oldPost.Body = postDto.Body;
           // _mapper.Map(postDto, oldPost);

            await _postsRepository.UpdateAsync(oldPost);
            return Ok(new UpdatePostDTO(oldPost.Name, oldPost.Description));
            //return Ok(_mapper.Map<PostDTO>(oldPost));
        }

        [HttpDelete("{postId}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult> DeleteAsync(int topicId, int postId)
        {
            var post = await _postsRepository.GetAsync(topicId, postId);
            if (post == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, PolicyNames.SameUser);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _postsRepository.DeleteAsync(post);

            // 204
            return NoContent();
        }
    }
}
