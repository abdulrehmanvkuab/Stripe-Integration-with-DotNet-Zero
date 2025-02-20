using System;

namespace Arch.Notifications.Dto
{
    public class GetPublishedNotificationsInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}