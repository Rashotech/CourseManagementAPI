namespace CourseManagement.Services.Interfaces
{
    public interface INotificationsService
    {
        Task SendNotificationAsync(string notification, bool alert);
    }
}