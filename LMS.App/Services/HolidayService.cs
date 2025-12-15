using LMS.App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LMS.App.Services;

public class HolidayService
{
    private readonly IMongoCollection<Holiday> _holidays;

    public HolidayService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _holidays = mongoDatabase.GetCollection<Holiday>("Holidays");
    }

    public async Task<List<Holiday>> GetAllAsync()
    {
        return await _holidays.Find(_ => true).SortBy(h => h.Date).ToListAsync();
    }

    public async Task<List<Holiday>> GetActiveHolidaysAsync()
    {
        return await _holidays.Find(h => h.IsActive).SortBy(h => h.Date).ToListAsync();
    }

    public async Task<Holiday?> GetByIdAsync(string id)
    {
        return await _holidays.Find(h => h.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Holiday>> GetHolidaysByYearAsync(int year)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31, 23, 59, 59);
        
        return await _holidays.Find(h => h.Date >= startDate && h.Date <= endDate && h.IsActive)
                              .SortBy(h => h.Date)
                              .ToListAsync();
    }

    public async Task<List<Holiday>> GetHolidaysByMonthAsync(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        
        return await _holidays.Find(h => h.Date >= startDate && h.Date <= endDate && h.IsActive)
                              .SortBy(h => h.Date)
                              .ToListAsync();
    }

    public async Task<Holiday> CreateAsync(Holiday holiday)
    {
        holiday.CreatedAt = DateTime.UtcNow;
        await _holidays.InsertOneAsync(holiday);
        return holiday;
    }

    public async Task UpdateAsync(string id, Holiday holiday)
    {
        holiday.UpdatedAt = DateTime.UtcNow;
        await _holidays.ReplaceOneAsync(h => h.Id == id, holiday);
    }

    public async Task DeleteAsync(string id)
    {
        await _holidays.DeleteOneAsync(h => h.Id == id);
    }

    public async Task<int> GetHolidayCountForMonthAsync(int year, int month)
    {
        var holidays = await GetHolidaysByMonthAsync(year, month);
        return holidays.Count;
    }
}