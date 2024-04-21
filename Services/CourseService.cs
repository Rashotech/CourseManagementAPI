using AutoMapper;
using CourseManagement.Database.Models;
using CourseManagement.Exceptions;
using CourseManagement.Models.Dto.Course;
using CourseManagement.Repositories.Interfaces;
using CourseManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CourseService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
        )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Guid> AddCourseAsync(CourseRequestDto courseRequestDto, string InstructorId)
        {
            await ValidateCourseRequestAsync(courseRequestDto);

            var course = _mapper.Map<Course>(courseRequestDto);
            course.InstructorId = InstructorId;

            await _unitOfWork.Courses.Add(course);
            await _unitOfWork.CommitAsync();

            return course.Id;
        }

        public async Task DeleteCourseAsync(Guid CourseId, string InstructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            if (course.InstructorId != InstructorId)
                throw new UnauthorizedAccessException("You are not authorized to delete this course");

            await _unitOfWork.Courses.Delete(CourseId);
            await _unitOfWork.CommitAsync();
        }

        public async Task EditCourseAsync(Guid CourseId, string InstructorId, CourseRequestDto courseRequestDto)
        {
            await ValidateCourseRequestAsync(courseRequestDto);
            var course = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            if (course.InstructorId != InstructorId)
                throw new UnauthorizedAccessException("You are not authorized to edit this course");

            _mapper.Map(courseRequestDto, course);
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<CourseReponseDto>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.GetAllCoursesAsync();
            var result = _mapper.Map<List<CourseReponseDto>>(courses);
            return result;
        }

        public async Task<List<CourseReponseDto>> GetCoursesByInstructorAsync(string InstructorId)
        {
            var courses = await _unitOfWork.Courses.GetCoursesByInstructorAsync(InstructorId);
            var result = _mapper.Map<List<CourseReponseDto>>(courses);
            return result;
        }

        public async Task<CourseReponseDto> GetSingleCourseByIdAsync(Guid CourseId)
        {
            var course = await _unitOfWork.Courses.GetSingleCourseByIdAsync(CourseId);
            var result = _mapper.Map<CourseReponseDto>(course);
            return result;
        }

        public async Task<ApplicationUser> GetUserInfo(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId) ?? throw new NotFoundException("User Not Found");
            return user;
        }
        public async Task<bool> ValidateCourseRequestAsync(CourseRequestDto courseRequestDto)
        {
            var validator = new CourseRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(courseRequestDto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(err => err.ErrorMessage)
                    .Aggregate((allErrors, nextError) => $"{allErrors}\n- {nextError}");

                throw new BadRequestException(errors);
            }

            return true;
        }
    }
}