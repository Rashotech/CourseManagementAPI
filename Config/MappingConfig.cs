using AutoMapper;
using CourseManagement.Database.Models;
using CourseManagement.Models.Dto;
using CourseManagement.Models.Dto.Course;

namespace CourseManagement.Config
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
		{
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, UserAuthResponseDto>().ReverseMap();
            CreateMap<CourseRequestDto, Course>().ReverseMap();
            CreateMap<CourseReponseDto, Course>().ReverseMap();
        }
    }
}