using LMS.App.Models;
using LMS.App.Services;

namespace LMS.App.Services;

public class WorkHoursService
{
    private readonly HolidayService _holidayService;
    private readonly EmployeeLeaveDetailsService _leaveService;
    private readonly EmployeeService _employeeService;
    private const int STANDARD_WORK_HOURS_PER_DAY = 8;

    public WorkHoursService(HolidayService holidayService, EmployeeLeaveDetailsService leaveService, EmployeeService employeeService)
    {
        _holidayService = holidayService;
        _leaveService = leaveService;
        _employeeService = employeeService;
    }

    public async Task<WorkHoursCalculation> CalculateWorkHoursAsync(string employeeId, int year, int month)
    {
        var totalDaysInMonth = DateTime.DaysInMonth(year, month);
        var workingDays = 0;
        var weekendDays = 0;
        var holidayDays = 0;
        var leaveDays = 0;

        // Get holidays for the month and filter by employee's team region
        var holidays = await _holidayService.GetHolidaysByMonthAsync(year, month);
        var employee = await _employeeService.GetByIdAsync(employeeId);
        var holidayDates = (employee == null
            ? holidays
            : holidays.Where(h => h.IndiaTeam == employee.IndiaTeam))
            .Select(h => h.Date.Date)
            .ToHashSet();

        // Get leaves for the employee in the month
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var leaves = await _leaveService.GetEmployeeLeavesInDateRangeAsync(employeeId, startDate, endDate);
        var leaveDates = new HashSet<DateTime>();

        foreach (var leave in leaves.Where(l => l.Status == LeaveStatus.Approved))
        {
            var currentDate = leave.StartDate.Date;
            while (currentDate <= leave.EndDate.Date)
            {
                leaveDates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }
        }

        // Calculate working days
        for (int day = 1; day <= totalDaysInMonth; day++)
        {
            var currentDate = new DateTime(year, month, day);
            var dayOfWeek = currentDate.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                weekendDays++;
            }
            else if (holidayDates.Contains(currentDate))
            {
                holidayDays++;
            }
            else if (leaveDates.Contains(currentDate))
            {
                leaveDays++;
            }
            else
            {
                workingDays++;
            }
        }

        var totalWorkHours = workingDays * STANDARD_WORK_HOURS_PER_DAY;

        return new WorkHoursCalculation
        {
            EmployeeId = employeeId,
            Year = year,
            Month = month,
            TotalDaysInMonth = totalDaysInMonth,
            WorkingDays = workingDays,
            WeekendDays = weekendDays,
            HolidayDays = holidayDays,
            LeaveDays = leaveDays,
            TotalWorkHours = totalWorkHours,
            StandardHoursPerDay = STANDARD_WORK_HOURS_PER_DAY
        };
    }
}

public class WorkHoursCalculation
{
    public string EmployeeId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalDaysInMonth { get; set; }
    public int WorkingDays { get; set; }
    public int WeekendDays { get; set; }
    public int HolidayDays { get; set; }
    public int LeaveDays { get; set; }
    public int TotalWorkHours { get; set; }
    public int StandardHoursPerDay { get; set; }
    
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");
}
