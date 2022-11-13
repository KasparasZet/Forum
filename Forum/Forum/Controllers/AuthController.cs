using AutoMapper;
using Forum.Auth;
using Forum.Auth.Model;
using Forum.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;

        public AuthController(UserManager<Users> userManager, IMapper mapper, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var user = await _userManager.FindByNameAsync(registerDTO.UserName);
            if (user != null)
                // Or more generic error to avoid exposing too much information
                return BadRequest("Request invalid.");

            var newUser = new Users
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if (!createUserResult.Succeeded)
                return BadRequest("Could not create a user.");

            await _userManager.AddToRoleAsync(newUser, UserRoles.SimpleUser);
            return CreatedAtAction(nameof(Register), _mapper.Map<UserDTO>(newUser));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("User name or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("User name or password is invalid.");

            var accessToken = await _tokenManager.CreateAccessTokenAsync(user);

            return Ok(new SuccessfulLoginResponseDTO(accessToken));
        }
    }
}
