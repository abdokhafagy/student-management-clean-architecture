using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystemApi.Data.Entities;

public class Payment
{
    [Key]
    public string Id { get; set; } = default!;

    [Range(0, 500)]
    public int price { get; set; }


    [Required]
    public string StudentId { get; set; } = default!;
    public Student Student { get; set; } = default!;
   
    [Required]
    public string MonthId { get; set; } = default!;
    public Month Month { get; set; } = default!;



    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UdatedAt { get; set; }

}

