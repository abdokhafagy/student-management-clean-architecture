using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Month
{
  
    [Key]
    public string Id { get; set; } = default!;
    [MaxLength(50)]
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public virtual List<Payment> Payments { get; set; } = new();
    public virtual List<Lesson> Lessons { get; set; } = new();
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