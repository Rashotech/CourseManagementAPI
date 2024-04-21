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

        public async Task<Course> AddCourseAsync(AddCourseRequestDto addCourseRequestDto, string InstructorId)
        {
            var course = new Course
            {
                CourseName = addCourseRequestDto.CourseName,
                Description = addCourseRequestDto.Description,
                InstructorId = InstructorId,
                StartDate = addCourseRequestDto.StartDate,
                EndDate = addCourseRequestDto.EndDate,
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

        public async Task EditCourseAsync(Guid CourseId, string InstructorId, EditCourseRequestDto editCourseRequestDto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            if(course.InstructorId != InstructorId)
                throw new UnauthorizedAccessException("You are not authorized to edit this course");

            _mapper.Map(editCourseRequestDto, course);
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.CommitAsync();
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
    }
}