using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Group
{
    [Key]
    public string Id { get; set; } = default!;
    [MaxLength(50)]
    public string GroupName { get; set; } = default!;
    public byte AcademicYear { get; set; }
    [MaxLength(50)]
    public string LessonDate { get; set; } = default!;
    public DateTime CreatedAt { get; set; }


    public virtual List<Student> Students { get; set; } = new();
    public virtual List<Lesson> Lessons { get; set; } = new();


}


