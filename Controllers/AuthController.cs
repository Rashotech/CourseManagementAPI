using System.Net;
using CourseManagement.Models;
using CourseManagement.Models.Dto;
using CourseManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService
        )
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse<UserAuthResponseDto>>> Register([FromBody] RegisterUserRequestDto registerUserRequestDto)
        {
            var user = await _authService.RegisterAsync(registerUserRequestDto);
            return CreatedAtAction(nameof(Login), null, new APIResponse<UserAuthResponseDto>
            {
                StatusCode = HttpStatusCode.Created,
                Message = "User registered successfully",
                Data = user
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse<UserAuthResponseDto>>> Login([FromBody] LoginUserRequestDto loginUserDto)
        {
            var user = await _authService.LoginAsync(loginUserDto);
            return new APIResponse<UserAuthResponseDto> { Data = user, Message = "Login Successful" };
        }
    }
}