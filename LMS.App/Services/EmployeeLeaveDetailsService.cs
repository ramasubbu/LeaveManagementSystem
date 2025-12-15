using LMS.App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LMS.App.Services
{
    public class EmployeeLeaveDetailsService
    {
        private readonly IMongoCollection<EmployeeLeaveDetails> _employeeLeaveDetails;
        private readonly EmployeeService _employeeService;

        public EmployeeLeaveDetailsService(IOptions<MongoDbSettings> mongoDbSettings, EmployeeService employeeService)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _employeeLeaveDetails = mongoDatabase.GetCollection<EmployeeLeaveDetails>(mongoDbSettings.Value.EmployeeLeaveDetailsCollectionName);
            _employeeService = employeeService;
        }

        public async Task<List<EmployeeLeaveDetails>> GetAllAsync()
        {
            var leaveDetails = await _employeeLeaveDetails.Find(x => x.IsActive).SortByDescending(x => x.AppliedDate).ToListAsync();
            
            // Populate employee information
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }

        public async Task<EmployeeLeaveDetails?> GetByIdAsync(string id)
        {
            var leaveDetail = await _employeeLeaveDetails.Find(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            
            if (leaveDetail != null)
            {
                leaveDetail.Employee = await _employeeService.GetByIdAsync(leaveDetail.EmployeeId);
            }
            
            return leaveDetail;
        }

        public async Task<List<EmployeeLeaveDetails>> GetByEmployeeIdAsync(string employeeId)
        {
            var leaveDetails = await _employeeLeaveDetails.Find(x => x.EmployeeId == employeeId && x.IsActive)
                .SortByDescending(x => x.AppliedDate).ToListAsync();
            
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }

        public async Task<List<EmployeeLeaveDetails>> GetByStatusAsync(LeaveStatus status)
        {
            var leaveDetails = await _employeeLeaveDetails.Find(x => x.Status == status && x.IsActive)
                .SortByDescending(x => x.AppliedDate).ToListAsync();
            
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }

        public async Task<List<EmployeeLeaveDetails>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<EmployeeLeaveDetails>.Filter.And(
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeLeaveDetails>.Filter.Or(
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, endDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.EndDate, endDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, endDate)
                    )
                )
            );

            var leaveDetails = await _employeeLeaveDetails.Find(filter).SortByDescending(x => x.AppliedDate).ToListAsync();
            
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }

        public async Task<List<EmployeeLeaveDetails>> SearchAsync(string searchTerm)
        {
            var filter = Builders<EmployeeLeaveDetails>.Filter.And(
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeLeaveDetails>.Filter.Or(
                    Builders<EmployeeLeaveDetails>.Filter.Regex(x => x.Reason, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<EmployeeLeaveDetails>.Filter.Regex(x => x.Comments, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            );

            var leaveDetails = await _employeeLeaveDetails.Find(filter).SortByDescending(x => x.AppliedDate).ToListAsync();
            
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }

        public async Task CreateAsync(EmployeeLeaveDetails employeeLeaveDetails)
        {
            employeeLeaveDetails.CreatedDate = DateTime.Now;
            employeeLeaveDetails.UpdatedDate = DateTime.Now;
            employeeLeaveDetails.AppliedDate = DateTime.Now;
            await _employeeLeaveDetails.InsertOneAsync(employeeLeaveDetails);
        }

        public async Task UpdateAsync(string id, EmployeeLeaveDetails employeeLeaveDetails)
        {
            employeeLeaveDetails.UpdatedDate = DateTime.Now;
            await _employeeLeaveDetails.ReplaceOneAsync(x => x.Id == id, employeeLeaveDetails);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<EmployeeLeaveDetails>.Update
                .Set(x => x.IsActive, false)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employeeLeaveDetails.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task ApproveLeaveAsync(string id, string approvedBy, string? approvalComments = null)
        {
            var update = Builders<EmployeeLeaveDetails>.Update
                .Set(x => x.Status, LeaveStatus.Approved)
                .Set(x => x.ApprovedBy, approvedBy)
                .Set(x => x.ApprovedDate, DateTime.Now)
                .Set(x => x.ApprovalComments, approvalComments)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employeeLeaveDetails.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task RejectLeaveAsync(string id, string rejectedBy, string? rejectionComments = null)
        {
            var update = Builders<EmployeeLeaveDetails>.Update
                .Set(x => x.Status, LeaveStatus.Rejected)
                .Set(x => x.ApprovedBy, rejectedBy)
                .Set(x => x.ApprovedDate, DateTime.Now)
                .Set(x => x.ApprovalComments, rejectionComments)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employeeLeaveDetails.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task CancelLeaveAsync(string id)
        {
            var update = Builders<EmployeeLeaveDetails>.Update
                .Set(x => x.Status, LeaveStatus.Cancelled)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employeeLeaveDetails.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task<bool> HasOverlappingLeaveAsync(string employeeId, DateTime startDate, DateTime endDate, string? excludeId = null)
        {
            var filter = Builders<EmployeeLeaveDetails>.Filter.And(
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.EmployeeId, employeeId),
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeLeaveDetails>.Filter.In(x => x.Status, new[] { LeaveStatus.Approved, LeaveStatus.Pending }),
                Builders<EmployeeLeaveDetails>.Filter.Or(
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, startDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, endDate),
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, endDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.EndDate, endDate)
                    )
                )
            );

            if (!string.IsNullOrEmpty(excludeId))
            {
                filter = Builders<EmployeeLeaveDetails>.Filter.And(
                    filter,
                    Builders<EmployeeLeaveDetails>.Filter.Ne(x => x.Id, excludeId)
                );
            }

            var count = await _employeeLeaveDetails.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<int> GetTotalLeaveDaysAsync(string employeeId, int year, LeaveType? leaveType = null)
        {
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = new DateTime(year, 12, 31);

            var filterBuilder = Builders<EmployeeLeaveDetails>.Filter;
            var filter = filterBuilder.And(
                filterBuilder.Eq(x => x.EmployeeId, employeeId),
                filterBuilder.Eq(x => x.IsActive, true),
                filterBuilder.Eq(x => x.Status, LeaveStatus.Approved),
                filterBuilder.Gte(x => x.StartDate, startOfYear),
                filterBuilder.Lte(x => x.EndDate, endOfYear)
            );

            if (leaveType.HasValue)
            {
                filter = filterBuilder.And(filter, filterBuilder.Eq(x => x.LeaveType, leaveType.Value));
            }

            var leaves = await _employeeLeaveDetails.Find(filter).ToListAsync();
            return leaves.Sum(l => l.TotalDays);
        }

        public async Task<List<EmployeeLeaveDetails>> GetEmployeeLeavesInDateRangeAsync(string employeeId, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<EmployeeLeaveDetails>.Filter.And(
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.EmployeeId, employeeId),
                Builders<EmployeeLeaveDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeLeaveDetails>.Filter.Or(
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, endDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.EndDate, endDate)
                    ),
                    Builders<EmployeeLeaveDetails>.Filter.And(
                        Builders<EmployeeLeaveDetails>.Filter.Lte(x => x.StartDate, startDate),
                        Builders<EmployeeLeaveDetails>.Filter.Gte(x => x.EndDate, endDate)
                    )
                )
            );

            var leaveDetails = await _employeeLeaveDetails.Find(filter).SortByDescending(x => x.AppliedDate).ToListAsync();
            
            foreach (var detail in leaveDetails)
            {
                detail.Employee = await _employeeService.GetByIdAsync(detail.EmployeeId);
            }
            
            return leaveDetails;
        }
    }
}
