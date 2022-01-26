using System;

namespace MailHub.Services.CleanupService.Configuration
{
    public class CleanupOptions
    {
        public TimeSpan CleanupInterval { get; set; }
        public int RetentionPeriodInHours { get; set; }
    }
}
