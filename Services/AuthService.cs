using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CourseManagement.Database.Models;
using CourseManagement.Exceptions;
using CourseManagement.Models;
using CourseManagement.Models.Dto;
using CourseManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CourseManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IConfiguration configuration
        )
		{
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<UserAuthResponseDto> LoginAsync(LoginUserRequestDto loginUserRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDto.Email) ?? throw new NotFoundException("User Not Found");
            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginUserRequestDto.Password);
            
            if(!isValidPassword)
                throw new BadRequestException("Invalid Credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? throw new NotFoundException("Role Not Found");

            return new UserAuthResponseDto()
            {
                Token = GenerateToken(user, role),
                User = _mapper.Map<UserDto>(user),
                Role = role
            };
        }

        public async Task<UserAuthResponseDto> RegisterAsync(RegisterUserRequestDto registerUserDto)
        {
            // Check if username is already taken
            var existingUser = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (existingUser != null)
                throw new BadRequestException("Username already exists. Please choose another username.");

            // Check if email is already registered
            var existingEmailUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (existingEmailUser != null)
                throw new BadRequestException("Email address is already registered. Please use another email.");

            ApplicationUser user = new()
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException("Unable to Register User. " + errorMessage);
            }

            var role = Roles.RegularUser;
            await _userManager.AddToRoleAsync(user, role);
            return new UserAuthResponseDto()
            {
                Token = GenerateToken(user, role),
                Role = role,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public string GenerateToken(ApplicationUser user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration["JWT:Secret"] ?? throw new NotFoundException("JWT Secret Not Present");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var _token = tokenHandler.CreateToken(tokenDescriptor);
            var token =  tokenHandler.WriteToken(_token);

            return token;
        }
    }
}