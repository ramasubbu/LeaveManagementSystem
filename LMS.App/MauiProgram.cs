using Microsoft.Extensions.Logging;
using LMS.App.Models;
using LMS.App.Services;

namespace LMS.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Configure MongoDB settings
            builder.Services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = "mongodb+srv://ramasubbuphoto_db_user:XmstlMJqfb1jkwq1@cluster0.jrdmy83.mongodb.net/";
                options.DatabaseName = "LeaveManagementDB";
                options.EmployeesCollectionName = "Employees";
                options.EmployeeProjectDetailsCollectionName = "EmployeeProjectDetails";
                options.EmployeeLeaveDetailsCollectionName = "EmployeeLeaveDetails";
            });

            builder.Services.AddMauiBlazorWebView();

            // Add MongoDB services
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<EmployeeProjectDetailsService>();
            builder.Services.AddScoped<EmployeeLeaveDetailsService>();
            builder.Services.AddScoped<EmployeeProjectMappingService>();

            // Add other services
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddScoped<HolidayService>();
            builder.Services.AddScoped<WorkHoursService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
