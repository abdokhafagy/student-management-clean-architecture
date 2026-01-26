using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Attendance
{
    [Key]
    public string Id { get; set; } = default!;
    public bool AttendanceStatus { get; set; }

    public string StudentId { get; set; } = default!;
    public Student Student { get; set; } = default!;

    public string LessonId { get; set; } = default!;
    public Lesson Lesson { get; set; } = default!;

    public int ExamResult { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
