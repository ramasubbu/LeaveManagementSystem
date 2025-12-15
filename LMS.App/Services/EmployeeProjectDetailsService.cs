using LMS.App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LMS.App.Services
{
    public class EmployeeProjectDetailsService
    {
        private readonly IMongoCollection<EmployeeProjectDetails> _employeeProjectDetails;
        private readonly EmployeeService _employeeService;

        public EmployeeProjectDetailsService(IOptions<MongoDbSettings> mongoDbSettings, EmployeeService employeeService)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _employeeProjectDetails = mongoDatabase.GetCollection<EmployeeProjectDetails>(mongoDbSettings.Value.EmployeeProjectDetailsCollectionName);
            _employeeService = employeeService;
        }

        public async Task<List<EmployeeProjectDetails>> GetAllAsync()
        {
            var projectDetails = await _employeeProjectDetails.Find(x => x.IsActive).ToListAsync();
            
            
            return projectDetails;
        }

        public async Task<EmployeeProjectDetails?> GetByIdAsync(string id)
        {
            var projectDetail = await _employeeProjectDetails.Find(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            
            
            return projectDetail;
        }

        public async Task<List<EmployeeProjectDetails>> SearchAsync(string searchTerm)
        {
            var filter = Builders<EmployeeProjectDetails>.Filter.And(
                Builders<EmployeeProjectDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeProjectDetails>.Filter.Or(
                    Builders<EmployeeProjectDetails>.Filter.Regex(x => x.ProjectCode, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<EmployeeProjectDetails>.Filter.Regex(x => x.ProjectName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<EmployeeProjectDetails>.Filter.Regex(x => x.ProjectManager, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            );

            var projectDetails = await _employeeProjectDetails.Find(filter).ToListAsync();
            
            return projectDetails;
        }

        public async Task CreateAsync(EmployeeProjectDetails employeeProjectDetails)
        {
            employeeProjectDetails.CreatedDate = DateTime.Now;
            employeeProjectDetails.UpdatedDate = DateTime.Now;
            await _employeeProjectDetails.InsertOneAsync(employeeProjectDetails);
        }

        public async Task UpdateAsync(string id, EmployeeProjectDetails employeeProjectDetails)
        {
            employeeProjectDetails.UpdatedDate = DateTime.Now;
            await _employeeProjectDetails.ReplaceOneAsync(x => x.Id == id, employeeProjectDetails);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<EmployeeProjectDetails>.Update
                .Set(x => x.IsActive, false)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employeeProjectDetails.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task<bool> IsProjectCodeUniqueForEmployeeAsync(string employeeId, string projectCode, string? excludeId = null)
        {
            var filter = Builders<EmployeeProjectDetails>.Filter.And(
                Builders<EmployeeProjectDetails>.Filter.Eq(x => x.ProjectCode, projectCode),
                Builders<EmployeeProjectDetails>.Filter.Eq(x => x.IsActive, true)
            );

            if (!string.IsNullOrEmpty(excludeId))
            {
                filter = Builders<EmployeeProjectDetails>.Filter.And(
                    filter,
                    Builders<EmployeeProjectDetails>.Filter.Ne(x => x.Id, excludeId)
                );
            }

            var count = await _employeeProjectDetails.CountDocumentsAsync(filter);
            return count == 0;
        }

        public async Task<List<EmployeeProjectDetails>> GetActiveProjectsAsync()
        {
            var filter = Builders<EmployeeProjectDetails>.Filter.And(
                Builders<EmployeeProjectDetails>.Filter.Eq(x => x.IsActive, true),
                Builders<EmployeeProjectDetails>.Filter.Or(
                    Builders<EmployeeProjectDetails>.Filter.Eq(x => x.EndDate, null),
                    Builders<EmployeeProjectDetails>.Filter.Gte(x => x.EndDate, DateTime.Today)
                )
            );

            var projectDetails = await _employeeProjectDetails.Find(filter).ToListAsync();
            
            return projectDetails;
        }
    }
}
