using StudentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystemApi.Data.Entities;

public class Student : BaseEntity
{
    [Required]
    [MaxLength(75)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = default!;

    [Required]
    [MaxLength(20)]
    public string ParentPhone { get; set; } = default!;

    [MaxLength(200)]
    public string? Address { get; set; }  // ADD THIS

    [MaxLength(500)]
    public string? Comments { get; set; }

    public DateTime? DateOfBirth { get; set; }  // ADD THIS

    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Foreign keys
    [ForeignKey(nameof(Group))]
    public string GroupId { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;


    // Navigation properties
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
