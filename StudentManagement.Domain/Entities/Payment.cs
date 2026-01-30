using StudentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystemApi.Data.Entities;

public class Payment : BaseEntity
{

    [Required]
    [Column(TypeName = "decimal(10,2)")]  // FIX: Use decimal for money
    public decimal Price { get; set; }  // FIX: Change from int to decimal

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;  // ADD THIS

    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "Cash";  // ADD THIS

    [MaxLength(100)]
    public string? TransactionId { get; set; }  // ADD THIS

    [MaxLength(500)]
    public string? Notes { get; set; }  // ADD THIS

    public bool IsPaid { get; set; } = false;

    // Foreign keys
    [ForeignKey(nameof(Student))]
    public string StudentId { get; set; } = default!;
    public virtual Student Student { get; set; } = default!;

    [ForeignKey(nameof(Month))]
    public string MonthId { get; set; } = default!;
    public virtual Month Month { get; set; } = default!;

}

