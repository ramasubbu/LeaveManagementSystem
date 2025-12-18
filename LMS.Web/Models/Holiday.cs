using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using LMS.Web.Services;

namespace LMS.Web.Models;

public class Holiday
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "Holiday name is required")]
    [StringLength(100, ErrorMessage = "Holiday name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Holiday date is required")]
    [BsonElement("Date")]
    public DateTime DateUtc { get; set; }

    [BsonIgnore]
    [Required(ErrorMessage = "Holiday date is required")]
    public DateTime Date
    {
        get => DateTimeUtilityService.ToLocalDate(DateUtc);
        set => DateUtc = DateTimeUtilityService.ToUtcDate(value);
    }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Team Location is required")]
    [Display(Name = "Team Location")]
    [BsonRepresentation(BsonType.String)]
    public TeamRegion IndiaTeam { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [BsonIgnore]
    public DateTime CreatedAt
    {
        get => DateTimeUtilityService.ToLocal(CreatedAtUtc);
    }

    [BsonElement("UpdatedAt")]
    public DateTime? UpdatedAtUtc { get; set; }

    [BsonIgnore]
    public DateTime? UpdatedAt
    {
        get => UpdatedAtUtc.HasValue ? DateTimeUtilityService.ToLocal(UpdatedAtUtc.Value) : null;
        set => UpdatedAtUtc = value.HasValue ? DateTimeUtilityService.ToUtc(value.Value) : null;
    }
}