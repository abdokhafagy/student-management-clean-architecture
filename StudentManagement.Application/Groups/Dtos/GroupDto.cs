using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.Groups.Dtos;

public class GroupDto
{
    [Required]
    public string GroupName { get; set; } = default!;
    [Required]
    public byte AcademicYear { get; set; }
    [Required]
    public string LessonDate { get; set; } = default!;
}
