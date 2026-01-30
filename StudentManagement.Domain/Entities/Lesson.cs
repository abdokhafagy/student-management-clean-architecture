using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystemApi.Data.Entities;

public class Lesson : BaseEntity
{
    //[Required]
    //[MaxLength(200)]
    //public string Title { get; set; } = default!;  // ADD THIS

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsEnded => LocalDate.GetLocalDate() >= EndDate;

    // Foreign keys
    [ForeignKey(nameof(Group))]
    public string GroupId { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;

    [ForeignKey(nameof(Month))]
    public string MonthId { get; set; } = default!;
    public virtual Month Month { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

}

