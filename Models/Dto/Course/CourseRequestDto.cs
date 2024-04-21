using FluentValidation;

namespace CourseManagement.Models.Dto.Course
{
    public class CourseRequestDto: CourseDto
    {
        
    }

    public class CourseRequestDtoValidator : AbstractValidator<CourseRequestDto>
    {
        public CourseRequestDtoValidator()
        {
            RuleFor(dto => dto.CourseName).NotEmpty().WithMessage("Course name is required.");
            RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(dto => dto.StartDate).NotEmpty().WithMessage("Start date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date must be greater than or equal to current day.");
            RuleFor(dto => dto.EndDate).NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(dto => dto.StartDate).WithMessage("End date must be greater or equal to start date.");
        }
    }
}