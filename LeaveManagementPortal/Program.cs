using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using LeaveManagementPortal.Models;
using LeaveManagementPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = "mongodb+srv://ramasubbuphoto_db_user:XmstlMJqfb1jkwq1@cluster0.jrdmy83.mongodb.net/";
    options.DatabaseName = "LeaveManagementDB";
    options.EmployeesCollectionName = "Employees";
    options.EmployeeProjectDetailsCollectionName = "EmployeeProjectDetails";
    options.EmployeeLeaveDetailsCollectionName = "EmployeeLeaveDetails";
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add MongoDB services
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeProjectDetailsService>();
builder.Services.AddScoped<EmployeeLeaveDetailsService>();
builder.Services.AddScoped<EmployeeProjectMappingService>();

// Add new services
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<HolidayService>();
builder.Services.AddScoped<WorkHoursService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Map Blazor Hub first
app.MapBlazorHub();

// Map Razor Pages
app.MapRazorPages();

// Map fallback to the _Host page for all unmatched routes
app.MapFallbackToPage("/_Host");

app.Run();
