using LMS.Web.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LMS.Web.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _employees = mongoDatabase.GetCollection<Employee>(mongoDbSettings.Value.EmployeesCollectionName);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employees.Find(x => x.IsActive).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _employees.Find(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Employee?> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _employees.Find(x => x.EmployeeCode == employeeCode && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> SearchAsync(string searchTerm)
        {
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(x => x.IsActive, true),
                Builders<Employee>.Filter.Or(
                    Builders<Employee>.Filter.Regex(x => x.FirstName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Employee>.Filter.Regex(x => x.LastName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Employee>.Filter.Regex(x => x.EmployeeCode, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Employee>.Filter.Regex(x => x.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Employee>.Filter.Regex(x => x.Department, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            );

            return await _employees.Find(filter).ToListAsync();
        }

        public async Task CreateAsync(Employee employee)
        {
            employee.CreatedDate = DateTime.Now;
            employee.UpdatedDate = DateTime.Now;
            await _employees.InsertOneAsync(employee);
        }

        public async Task UpdateAsync(string id, Employee employee)
        {
            employee.UpdatedDate = DateTime.Now;
            await _employees.ReplaceOneAsync(x => x.Id == id, employee);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<Employee>.Update
                .Set(x => x.IsActive, false)
                .Set(x => x.UpdatedDate, DateTime.Now);
            
            await _employees.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode, string? excludeId = null)
        {
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(x => x.EmployeeCode, employeeCode),
                Builders<Employee>.Filter.Eq(x => x.IsActive, true)
            );

            if (!string.IsNullOrEmpty(excludeId))
            {
                filter = Builders<Employee>.Filter.And(
                    filter,
                    Builders<Employee>.Filter.Ne(x => x.Id, excludeId)
                );
            }

            var count = await _employees.CountDocumentsAsync(filter);
            return count == 0;
        }

        public async Task<List<Employee>> GetManagersAsync()
        {
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(x => x.IsActive, true),
                Builders<Employee>.Filter.Or(
                    Builders<Employee>.Filter.Regex(x => x.Designation, new MongoDB.Bson.BsonRegularExpression("senior technical manager", "i")),
                    Builders<Employee>.Filter.Regex(x => x.Designation, new MongoDB.Bson.BsonRegularExpression("technical manager", "i")),
                    Builders<Employee>.Filter.Regex(x => x.Designation, new MongoDB.Bson.BsonRegularExpression("senior architect", "i"))
                )
            );

            return await _employees.Find(filter).ToListAsync();
        }
    }
}