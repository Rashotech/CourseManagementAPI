using System.Net;
using System.Security.Claims;
using CourseManagement.Models;
using CourseManagement.Models.Dto.Course;
using CourseManagement.Services.Interfaces;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController: ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IServerSentEventsService _serverSentEventsService;
        private const string HEARTBEAT_MESSAGE_FORMAT = "Demo.AspNetCore.ServerSentEvents Heartbeat ({0} UTC)";

        public CourseController(ICourseService courseService, IServerSentEventsService serverSentEventsService)
        {
            _courseService = courseService;
            _serverSentEventsService = serverSentEventsService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse<Guid>>> AddCourse([FromBody] CourseRequestDto courseRequestDto)
        {
            var InstructorId = GetLoggedInUserID();
            var courseId = await _courseService.AddCourseAsync(courseRequestDto, InstructorId);
            return CreatedAtAction(nameof(GetSingleCourseById), new { Id = courseId }, new APIResponse<Guid>
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Course Added Successfully",
                Data = courseId
            });
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse<List<CourseReponseDto>>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(new APIResponse<List<CourseReponseDto>> { Data = courses, Message = "All Courses Fetched Successfully" });
        }

        [HttpGet("send-heartbeat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseBase>> SendHeartBeat()
        {
            var notification = "Heartbeat sent successfully";
            await _serverSentEventsService.SendEventAsync(new ServerSentEvent
            {
                Type ="alert",
                Data = new List<string>(notification.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            });

            await _serverSentEventsService.SendEventAsync(String.Format(HEARTBEAT_MESSAGE_FORMAT, DateTime.UtcNow));

            var clients = _serverSentEventsService.GetClients();

            return Ok(new APIResponseBase {  Message = "Notification sent Successfully", Data = clients.Count() });
        }

        [HttpGet("instructors/{id}")] 
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse<List<CourseReponseDto>>>> GetCoursesByInstructor(string id)
        {
            var courses = await _courseService.GetCoursesByInstructorAsync(id);
            return Ok(new APIResponse<List<CourseReponseDto>> { Data = courses, Message = "Instructor Courses Fetched Successfully" });
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse<CourseReponseDto>>> GetSingleCourseById(Guid id)
        {
            var course = await _courseService.GetSingleCourseByIdAsync(id);
            return Ok(new APIResponse<CourseReponseDto> { Data = course, Message = "Course Fetched Successfully" });
        }

        [HttpPatch("{id}/edit")]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditCourseById(Guid id, [FromBody] CourseRequestDto courseRequestDto)
        {
            var InstructorId = GetLoggedInUserID();
            await _courseService.EditCourseAsync(id, InstructorId, courseRequestDto);
            return NoContent();
        }

        [HttpPost("{id}/delete")]
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