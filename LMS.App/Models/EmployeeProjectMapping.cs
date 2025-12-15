using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LMS.App.Models
{
    public class EmployeeProjectMapping
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        [BsonElement("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project is required")]
        [BsonElement("projectId")]
        public string ProjectId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [BsonElement("role")]
        public string Role { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Allocation percentage must be between 1 and 100")]
        [BsonElement("allocationPercentage")]
        public int AllocationPercentage { get; set; } = 100;

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties (not stored in MongoDB)
        [BsonIgnore]
        public Employee? Employee { get; set; }

        [BsonIgnore]
        public EmployeeProjectDetails? Project { get; set; }
    }
}