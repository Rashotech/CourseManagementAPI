using CourseManagement.Models.Dto;

namespace CourseManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserAuthResponseDto> RegisterAsync(RegisterUserRequestDto registerUserDto, bool isAdmin = false);
        Task<UserAuthResponseDto> LoginAsync(LoginUserRequestDto loginUserDto);
    }
}