using StudentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Month : BaseEntity
{

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = default!;

    [Required]
    public int Year { get; set; }  // ADD THIS: To distinguish Jan 2024 vs Jan 2025

    public bool IsActive { get; set; } = true;

    public int Order { get; set; }  // For sorting (1=January, 2=February, etc.)

    // Navigation properties
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
/*
 * public class Month
{
    public int MonthId { get; set; }  // Corresponds to month_id
    public string MonthName { get; set; }  // Corresponds to month_name
    public int Year { get; set; }  // Corresponds to year
    public DateTime CreatedAt { get; set; }  // Corresponds to created_at

    public virtual ICollection<Lesson> Lessons { get; set; }  // Navigation property for lessons
}
 */