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

        public async Task<Course> AddCourseAsync(CourseRequestDto courseRequestDto, string InstructorId)
        {
            await ValidateCourseRequestAsync(courseRequestDto);
            var course = new Course
            {
                CourseName = courseRequestDto.CourseName,
                Description = courseRequestDto.Description,
                InstructorId = InstructorId,
                StartDate = courseRequestDto.StartDate,
                EndDate = courseRequestDto.EndDate
            };

            await _unitOfWork.Courses.Add(course);
            await _unitOfWork.CommitAsync();

            return course;
        }

        public async Task DeleteCourseAsync(Guid CourseId, string InstructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            if(course.InstructorId != InstructorId)
                throw new UnauthorizedAccessException("You are not authorized to delete this course");

            await _unitOfWork.Courses.Delete(CourseId);
            await _unitOfWork.CommitAsync();
        }

        public async Task EditCourseAsync(Guid CourseId, string InstructorId, CourseRequestDto courseRequestDto)
        {
            await ValidateCourseRequestAsync(courseRequestDto);
            var course = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            if(course.InstructorId != InstructorId)
                throw new UnauthorizedAccessException("You are not authorized to edit this course");

            _mapper.Map(courseRequestDto, course);
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.CommitAsync();
        }

        public Task GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetCoursesByInstructorAsync(string InstructorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Course> GetSingleCourseByIdAsync(Guid CourseId)
        {
            var course = await _unitOfWork.Courses.GetSingleCourseByIdAsync(CourseId);
            return course;
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