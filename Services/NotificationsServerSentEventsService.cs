using CourseManagement.Services.Interfaces;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;

namespace CourseManagement.Services
{
    internal class NotificationsServerSentEventsService : ServerSentEventsService, INotificationsServerSentEventsService
    {
        public NotificationsServerSentEventsService(IOptions<ServerSentEventsServiceOptions<NotificationsServerSentEventsService>> options)
            : base(options.ToBaseServerSentEventsServiceOptions())
        { }
    }
}