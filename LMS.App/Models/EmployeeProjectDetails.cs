using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using LMS.App.Services;

namespace LMS.App.Models
{
    public class EmployeeProjectDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

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
        [BsonElement("StartDate")]
        public DateTime StartDateUtc { get; set; } = DateTimeUtilityService.UtcToday;

        [BsonIgnore]
        [Display(Name = "Start Date")]
        public DateTime StartDate
        {
            get => DateTimeUtilityService.ToLocalDate(StartDateUtc);
            set => StartDateUtc = DateTimeUtilityService.ToUtcDate(value);
        }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [BsonElement("EndDate")]
        public DateTime? EndDateUtc { get; set; }

        [BsonIgnore]
        [Display(Name = "End Date")]
        public DateTime? EndDate
        {
            get => EndDateUtc.HasValue ? DateTimeUtilityService.ToLocalDate(EndDateUtc.Value) : null;
            set => EndDateUtc = value.HasValue ? DateTimeUtilityService.ToUtcDate(value.Value) : null;
        }

        [Display(Name = "Project Manager")]
        public string ProjectManager { get; set; } = string.Empty;

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
        public DateTime UpdatedDateUtc { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate
        {
            get => DateTimeUtilityService.ToLocal(UpdatedDateUtc);
            set => UpdatedDateUtc = DateTimeUtilityService.ToUtc(value);
        }

        // Navigation property for Employee (will be populated manually)
        [BsonIgnore]
        public Employee? Employee { get; set; }
    }
}
