using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementPortal.Models
{
    public class EmployeeProjectDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee ID is required")]
        [Display(Name = "Employee ID")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Code is required")]
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Name is required")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        [Display(Name = "Allocation Percentage")]
        [Range(0, 100, ErrorMessage = "Allocation percentage must be between 0 and 100")]
        public decimal AllocationPercentage { get; set; } = 100;

        [Display(Name = "Project Manager")]
        public string ProjectManager { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation property for Employee (will be populated manually)
        [BsonIgnore]
        public Employee? Employee { get; set; }
    }
}