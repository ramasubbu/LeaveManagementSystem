using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using LMS.App.Services;

namespace LMS.App.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee Code is required")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "SLB Email ID")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string SlbEmailId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Team Location is required")]
        [Display(Name = "Team Location")]
        [BsonRepresentation(BsonType.String)]
        public TeamRegion IndiaTeam { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; } = string.Empty;

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        [BsonElement("DateOfJoining")]
        public DateTime DateOfJoiningUtc { get; set; } = DateTimeUtilityService.UtcToday;

        [BsonIgnore]
        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining
        {
            get => DateTimeUtilityService.ToLocalDate(DateOfJoiningUtc);
            set => DateOfJoiningUtc = DateTimeUtilityService.ToUtcDate(value);
        }

        [Display(Name = "Manager ID")]
        public string ManagerId { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        [BsonElement("CreatedDate")]
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate
        {
            get => DateTimeUtilityService.ToLocal(CreatedDateUtc);
        }

        [Display(Name = "Updated Date")]
        [BsonElement("UpdatedDate")]
        public DateTime? UpdatedDateUtc { get; set; }

        [BsonIgnore]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate
        {
            get => UpdatedDateUtc.HasValue ? DateTimeUtilityService.ToLocal(UpdatedDateUtc.Value) : null;
            set => UpdatedDateUtc = value.HasValue ? DateTimeUtilityService.ToUtc(value.Value) : null;
        }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
