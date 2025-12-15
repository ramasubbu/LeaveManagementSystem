using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using LMS.App.Services;

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
        public DateTime StartDateUtc { get; set; } = DateTimeUtilityService.UtcToday;

        [BsonIgnore]
        public DateTime StartDate
        {
            get => DateTimeUtilityService.ToLocalDate(StartDateUtc);
            set => StartDateUtc = DateTimeUtilityService.ToUtcDate(value);
        }

        [BsonElement("endDate")]
        public DateTime? EndDateUtc { get; set; }

        [BsonIgnore]
        public DateTime? EndDate
        {
            get => EndDateUtc.HasValue ? DateTimeUtilityService.ToLocalDate(EndDateUtc.Value) : null;
            set => EndDateUtc = value.HasValue ? DateTimeUtilityService.ToUtcDate(value.Value) : null;
        }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        public DateTime CreatedAt
        {
            get => DateTimeUtilityService.ToLocal(CreatedAtUtc);
        }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        public DateTime UpdatedAt
        {
            get => DateTimeUtilityService.ToLocal(UpdatedAtUtc);
            set => UpdatedAtUtc = DateTimeUtilityService.ToUtc(value);
        }

        // Navigation properties (not stored in MongoDB)
        [BsonIgnore]
        public Employee? Employee { get; set; }

        [BsonIgnore]
        public EmployeeProjectDetails? Project { get; set; }
    }
}
