using LMS.Web.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LMS.Web.Services
{
    public class EmployeeProjectMappingService
    {
        private readonly IMongoCollection<EmployeeProjectMapping> _mappingCollection;
        private readonly IMongoCollection<Employee> _employeeCollection;
        private readonly IMongoCollection<EmployeeProjectDetails> _projectCollection;

        public EmployeeProjectMappingService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            
            _mappingCollection = mongoDatabase.GetCollection<EmployeeProjectMapping>("EmployeeProjectMappings");
            _employeeCollection = mongoDatabase.GetCollection<Employee>("Employees");
            _projectCollection = mongoDatabase.GetCollection<EmployeeProjectDetails>("EmployeeProjectDetails");
        }

        public async Task<List<EmployeeProjectMapping>> GetAllAsync()
        {
            var mappings = await _mappingCollection.Find(x => true).ToListAsync();
            
            // Populate navigation properties
            foreach (var mapping in mappings)
            {
                mapping.Employee = await _employeeCollection.Find(e => e.Id == mapping.EmployeeId).FirstOrDefaultAsync();
                mapping.Project = await _projectCollection.Find(p => p.Id == mapping.ProjectId).FirstOrDefaultAsync();
            }
            
            return mappings;
        }

        public async Task<EmployeeProjectMapping?> GetByIdAsync(string id)
        {
            var mapping = await _mappingCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            if (mapping != null)
            {
                mapping.Employee = await _employeeCollection.Find(e => e.Id == mapping.EmployeeId).FirstOrDefaultAsync();
                mapping.Project = await _projectCollection.Find(p => p.Id == mapping.ProjectId).FirstOrDefaultAsync();
            }
            
            return mapping;
        }

        public async Task<List<EmployeeProjectMapping>> GetByEmployeeIdAsync(string employeeId)
        {
            var mappings = await _mappingCollection.Find(x => x.EmployeeId == employeeId).ToListAsync();
            
            foreach (var mapping in mappings)
            {
                mapping.Employee = await _employeeCollection.Find(e => e.Id == mapping.EmployeeId).FirstOrDefaultAsync();
                mapping.Project = await _projectCollection.Find(p => p.Id == mapping.ProjectId).FirstOrDefaultAsync();
            }
            
            return mappings;
        }

        public async Task<List<EmployeeProjectMapping>> GetByProjectIdAsync(string projectId)
        {
            var mappings = await _mappingCollection.Find(x => x.ProjectId == projectId).ToListAsync();
            
            foreach (var mapping in mappings)
            {
                mapping.Employee = await _employeeCollection.Find(e => e.Id == mapping.EmployeeId).FirstOrDefaultAsync();
                mapping.Project = await _projectCollection.Find(p => p.Id == mapping.ProjectId).FirstOrDefaultAsync();
            }
            
            return mappings;
        }

        public async Task<bool> IsEmployeeAlreadyAssignedToProjectAsync(string employeeId, string projectId, string? excludeId = null)
        {
            var filter = Builders<EmployeeProjectMapping>.Filter.And(
                Builders<EmployeeProjectMapping>.Filter.Eq(x => x.EmployeeId, employeeId),
                Builders<EmployeeProjectMapping>.Filter.Eq(x => x.ProjectId, projectId),
                Builders<EmployeeProjectMapping>.Filter.Eq(x => x.IsActive, true)
            );

            if (!string.IsNullOrEmpty(excludeId))
            {
                filter = Builders<EmployeeProjectMapping>.Filter.And(
                    filter,
                    Builders<EmployeeProjectMapping>.Filter.Ne(x => x.Id, excludeId)
                );
            }

            var count = await _mappingCollection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task CreateAsync(EmployeeProjectMapping mapping)
        {
            mapping.CreatedAt = DateTime.UtcNow;
            mapping.UpdatedAt = DateTime.UtcNow;
            await _mappingCollection.InsertOneAsync(mapping);
        }

        public async Task UpdateAsync(string id, EmployeeProjectMapping mapping)
        {
            mapping.UpdatedAt = DateTime.UtcNow;
            await _mappingCollection.ReplaceOneAsync(x => x.Id == id, mapping);
        }

        public async Task RemoveAsync(string id)
        {
            await _mappingCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<EmployeeProjectMapping>> SearchAsync(string searchTerm)
        {
            var mappings = await GetAllAsync();
            
            if (string.IsNullOrEmpty(searchTerm))
                return mappings;

            return mappings.Where(m => 
                (m.Employee?.FullName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (m.Employee?.EmployeeCode?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (m.Project?.ProjectName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (m.Project?.ProjectCode?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (m.Role?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();
        }
    }
}