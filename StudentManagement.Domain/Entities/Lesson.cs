using StudentManagement.Domain.Helper;
using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Lesson
{
    
    [Key]
    public string Id { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsEnded => LocalDate.GetLocalDate() >= EndDate;

    public string GroupId { get; set; } = default!;
    public Group Group { get; set; } = default!;

    public string MonthId { get; set; } = default!;
    public Month Month { get; set; } = default!;

    public virtual List<Attendance> Attendances { get; set; } = new();
}

