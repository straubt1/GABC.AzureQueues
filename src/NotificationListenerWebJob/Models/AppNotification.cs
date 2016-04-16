namespace NotificationListenerWebJob.Models
{
    public class AppNotification
    {
        /// <summary>
        /// Unique id for the notification, set by the notifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Who the notification is for
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Who the notification is from
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// The subject of the notification
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The content of the message
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Status of the notification
        /// </summary>
        public string Status { get; set; }
    }
}
