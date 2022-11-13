using AutoMapper;
using Forum.Auth.Model;
using Forum.Data.Entities;
using Forum.Data.Repositories;
using Forum.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forum.Controllers
{
    [ApiController]
    [Route("api/topics")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsRepository _topicsRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;


        public TopicsController(ITopicsRepository topicsRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _topicsRepository = topicsRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<IEnumerable<TopicDto>> GetAll()
        {
            return (await _topicsRepository.GetAll()).Select(o => _mapper.Map<TopicDto>(o));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDto>> Get(int id)
        {
            var topic = await _topicsRepository.Get(id);
            if (topic == null) return NotFound($"Topic with id '{id}' not found.");

            //return _mapper.Map<TopicDto>(topic);
            return Ok(_mapper.Map<TopicDto>(topic));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<TopicDto>> Post(CreateTopicDto topicDto)
        {
            //var topic = _mapper.Map<Topic>(topicDto);

            var topic = new Topic
            {
                Name = topicDto.Name,
                Description = topicDto.Description,
                CreationTimeUtc = DateTime.UtcNow,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            await _topicsRepository.Create(topic);

            // 201
            // Created topic
            return Created("", new TopicDto(topic.Id, topic.Name, topic.Description));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<TopicDto>> Put(int id, UpdateTopicDto topicDto)
        {
            var topic = await _topicsRepository.Get(id);
            if (topic == null) return NotFound($"Topic with id '{id}' not found.");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, topic, PolicyNames.SameUser);

            if (!authorizationResult.Succeeded)
            {
                // 403
                // 404
                // 401
                return Forbid();
            }

            topic.Name = topicDto.Name;
            await _topicsRepository.Put(topic);

            return Ok(new TopicDto(topic.Id, topic.Name, topic.Description));

            //topic.Name = topicDto.Name;
            //_mapper.Map(topicDto, topic);

            //await _topicsRepository.Put(topic);

            //return _mapper.Map<TopicDto>(topic);
           // return Ok(_mapper.Map<TopicDto>(topic));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.SimpleUser)]
        public async Task<ActionResult<TopicDto>> Delete(int id)
        {
            var topic = await _topicsRepository.Get(id);
            if (topic == null) return NotFound($"Topic with id '{id}' not found.");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, topic, PolicyNames.SameUser);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _topicsRepository.Delete(topic);

            // 204
            return NoContent();
        }
    }
}
