using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Models
{
    public enum LeaveType
    {
        [Display(Name = "Annual Leave")]
        Annual,
        [Display(Name = "Sick Leave")]
        Sick,
        [Display(Name = "Maternity Leave")]
        Maternity,
        [Display(Name = "Paternity Leave")]
        Paternity,
        [Display(Name = "Personal Leave")]
        Personal,
        [Display(Name = "Bereavement Leave")]
        Bereavement,
        [Display(Name = "Compensatory Leave")]
        Compensatory
    }

    public enum LeaveStatus
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Cancelled")]
        Cancelled
    }

    public class EmployeeLeaveDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee ID is required")]
        [Display(Name = "Employee ID")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Leave Type is required")]
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "End Date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Reason is required")]
        [Display(Name = "Reason for Leave")]
        public string Reason { get; set; } = string.Empty;

        [Display(Name = "Comments")]
        public string Comments { get; set; } = string.Empty;

        [Display(Name = "Total Days")]
        public int TotalDays => (EndDate - StartDate).Days + 1;

        [Display(Name = "Leave Status")]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        [Display(Name = "Applied Date")]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [Display(Name = "Approved/Rejected By")]
        public string? ApprovedBy { get; set; }

        [Display(Name = "Approved/Rejected Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Approval Comments")]
        public string? ApprovalComments { get; set; }

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