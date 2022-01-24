using System;

namespace Template.Services.CleanupService.Configuration
{
    public class CleanupOptions
    {
        public TimeSpan CleanupInterval { get; set; }
        public int RetentionPeriodInHours { get; set; }
    }
}
