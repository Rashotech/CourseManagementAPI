using System.Net;
using System.Security.Claims;
using CourseManagement.Database.Models;
using CourseManagement.Models;
using CourseManagement.Models.Dto.Course;
using CourseManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController: ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse<Course>>> AddCourse([FromBody] AddCourseRequestDto addCourseRequestDto)
        {
            var InstructorId = GetLoggedInUserID();
            var course = await _courseService.AddCourseAsync(addCourseRequestDto, InstructorId);
            return CreatedAtAction(nameof(GetSingleCourseById), new { Id = course.Id }, new APIResponse<Course>
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Course Added Successfully",
                Data = course
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse<Course>>> GetSingleCourseById(Guid id)
        {
            var course = await _courseService.GetSingleCourseByIdAsync(id);
            return Ok(new APIResponse<Course> { Data = course, Message = "Course Fetched Successfully" });
        }

        [HttpPatch("{id}/Edit")]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse<Course>>> EditCourseById(Guid id, [FromBody] EditCourseRequestDto editCourseRequestDto)
        {
            var InstructorId = GetLoggedInUserID();
            await _courseService.EditCourseAsync(id, InstructorId, editCourseRequestDto);
            return NoContent();
        }

        [HttpPost("{id}/Delete")]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponseBase>> DeleteCourseById(Guid id)
        {
            var InstructorId = GetLoggedInUserID();
            await _courseService.DeleteCourseAsync(id, InstructorId);
            return Ok(new APIResponseBase { Message = "Course Deleted Successfully" });
        }

        private string GetLoggedInUserID()
        {
            var userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not logged in");
            }
            return userId;
        }
    }
}