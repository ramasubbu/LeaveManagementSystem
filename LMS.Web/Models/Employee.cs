using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Models
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

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; } = string.Empty;

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; } = DateTime.Now;

        [Display(Name = "Manager ID")]
        public string ManagerId { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}