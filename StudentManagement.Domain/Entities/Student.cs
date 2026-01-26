using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Student
{
    [Key]
    public string Id { get; set; } = default!;
    [MaxLength(75)]
    public string Name { get; set; } = default!;
    [MaxLength(20)]
    public string Phone { get; set; } = default!;
    [MaxLength(20)]
    public string ParentPhone { get; set; } = default!;
    [MaxLength(150)]
    public string? Comments { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    //public StatusOfStudent.Status Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string GroupId { get; set; } = default!;
    public Group Group { get; set; } = default!;
    public virtual List<Attendance> Attendances { get; set; } = new();
    public virtual List<Payment> Payments { get; set; } = new();
}
