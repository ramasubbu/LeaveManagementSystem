using System.Globalization;

namespace LMS.App.Services
{
    public class DateTimeHelperService
    {
        /// <summary>
        /// Converts a DateTime from UTC to local timezone for display purposes
        /// </summary>
        /// <param name="utcDateTime">UTC DateTime from database</param>
        /// <returns>DateTime in local timezone</returns>
        public DateTime ConvertUtcToLocal(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
            {
                // Treat as UTC if kind is unspecified (common with MongoDB)
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            
            return utcDateTime.ToLocalTime();
        }

        /// <summary>
        /// Converts a local DateTime to UTC for database storage
        /// </summary>
        /// <param name="localDateTime">Local DateTime from UI</param>
        /// <returns>DateTime in UTC</returns>
        public DateTime ConvertLocalToUtc(DateTime localDateTime)
        {
            if (localDateTime.Kind == DateTimeKind.Unspecified)
            {
                // Treat as local time if kind is unspecified
                localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);
            }
            
            return localDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts a date-only value (from InputDate) to UTC start of day
        /// </summary>
        /// <param name="dateOnly">Date from date picker (treated as local timezone)</param>
        /// <returns>Start of day in UTC</returns>
        public DateTime ConvertDateOnlyToUtc(DateTime dateOnly)
        {
            // Create DateTime at start of day in local timezone
            var localDateTime = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, 0, 0, 0, DateTimeKind.Local);
            return localDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts UTC DateTime to date-only for display in date inputs
        /// </summary>
        /// <param name="utcDateTime">UTC DateTime from database</param>
        /// <returns>Date-only value in local timezone</returns>
        public DateTime ConvertUtcToDateOnly(DateTime utcDateTime)
        {
            var localDateTime = ConvertUtcToLocal(utcDateTime);
            return localDateTime.Date;
        }

        /// <summary>
        /// Formats UTC DateTime for display with timezone info
        /// </summary>
        /// <param name="utcDateTime">UTC DateTime from database</param>
        /// <param name="format">Optional format string</param>
        /// <returns>Formatted string with local time</returns>
        public string FormatForDisplay(DateTime utcDateTime, string format = "yyyy-MM-dd HH:mm")
        {
            var localDateTime = ConvertUtcToLocal(utcDateTime);
            return localDateTime.ToString(format);
        }

        /// <summary>
        /// Formats date-only for display
        /// </summary>
        /// <param name="utcDateTime">UTC DateTime from database</param>
        /// <param name="format">Optional format string</param>
        /// <returns>Formatted date string in local timezone</returns>
        public string FormatDateForDisplay(DateTime utcDateTime, string format = "yyyy-MM-dd")
        {
            var localDate = ConvertUtcToDateOnly(utcDateTime);
            return localDate.ToString(format);
        }

        /// <summary>
        /// Gets the current date in UTC
        /// </summary>
        /// <returns>Current UTC date at start of day</returns>
        public DateTime GetUtcToday()
        {
            return DateTime.UtcNow.Date;
        }

        /// <summary>
        /// Gets the current local date for UI display
        /// </summary>
        /// <returns>Current local date</returns>
        public DateTime GetLocalToday()
        {
            return DateTime.Today;
        }
    }
}