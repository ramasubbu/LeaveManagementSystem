namespace LeaveManagementPortal.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string EmployeesCollectionName { get; set; } = string.Empty;
        public string EmployeeProjectDetailsCollectionName { get; set; } = string.Empty;
        public string EmployeeLeaveDetailsCollectionName { get; set; } = string.Empty;
    }
}