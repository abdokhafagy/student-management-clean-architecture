using StudentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystemApi.Data.Entities;

public class Attendance: BaseEntity
{
    public bool AttendanceStatus { get; set; }

    public int ExamResult { get; set; }

    // Foreign keys
    [ForeignKey(nameof(Student))]
    public string StudentId { get; set; } = default!;
    public virtual Student Student { get; set; } = default!;

    [ForeignKey(nameof(Lesson))]
    public string LessonId { get; set; } = default!;
    public virtual Lesson Lesson { get; set; } = default!;



}
