using StudentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Group : BaseEntity
{

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = default!;

    public byte AcademicYear { get; set; }

    [Required]
    [MaxLength(50)]
    public string LessonDate { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();


}




