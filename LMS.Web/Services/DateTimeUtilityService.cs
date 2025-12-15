using System;

namespace LMS.Web.Services
{
    /// <summary>
    /// Service for handling UTC to Local timezone conversions
    /// All dates are stored in UTC in the database
    /// All dates are displayed in the user's local timezone in the UI
    /// </summary>
    public class DateTimeUtilityService
    {
        /// <summary>
        /// Convert local datetime to UTC for database storage
        /// </summary>
        public static DateTime ToUtc(DateTime localDateTime)
        {
            if (localDateTime.Kind == DateTimeKind.Utc)
                return localDateTime;
            
            // If unspecified, assume it's local time
            if (localDateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(localDateTime, DateTimeKind.Local).ToUniversalTime();
            
            return localDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Convert UTC datetime to local time for UI display
        /// </summary>
        public static DateTime ToLocal(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Local)
                return utcDateTime;
            
            // If unspecified, assume it's UTC
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc).ToLocalTime();
            
            return utcDateTime.ToLocalTime();
        }

        /// <summary>
        /// Convert UTC date to local date (for date-only fields)
        /// </summary>
        public static DateTime ToLocalDate(DateTime utcDate)
        {
            return ToLocal(utcDate).Date;
        }

        /// <summary>
        /// Convert local date to UTC (for date-only fields)
        /// </summary>
        public static DateTime ToUtcDate(DateTime localDate)
        {
            // For dates, we need to ensure we're working with the full day in UTC
            var localDateTime = localDate.Date; // Start of day
            return ToUtc(DateTime.SpecifyKind(localDateTime, DateTimeKind.Local));
        }

        /// <summary>
        /// Format UTC datetime as local time string
        /// </summary>
        public static string FormatToLocal(DateTime? utcDateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (utcDateTime == null)
                return string.Empty;
            
            return ToLocal(utcDateTime.Value).ToString(format);
        }

        /// <summary>
        /// Format UTC date as local date string
        /// </summary>
        public static string FormatDateToLocal(DateTime? utcDate, string format = "yyyy-MM-dd")
        {
            if (utcDate == null)
                return string.Empty;
            
            return ToLocalDate(utcDate.Value).ToString(format);
        }

        /// <summary>
        /// Get current time in UTC
        /// </summary>
        public static DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Get current date in UTC
        /// </summary>
        public static DateTime UtcToday => DateTime.UtcNow.Date;

        /// <summary>
        /// Get current time in local timezone
        /// </summary>
        public static DateTime LocalNow => ToLocal(DateTime.UtcNow);

        /// <summary>
        /// Get current date in local timezone
        /// </summary>
        public static DateTime LocalToday => ToLocal(DateTime.UtcNow).Date;
    }
}
