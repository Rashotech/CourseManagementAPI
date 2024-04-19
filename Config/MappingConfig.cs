using AutoMapper;
using CourseManagement.Database.Models;
using CourseManagement.Models.Dto;

namespace CourseManagement.Config
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
		{
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, LoginUserResponseDto>().ReverseMap();
        }
    }
}