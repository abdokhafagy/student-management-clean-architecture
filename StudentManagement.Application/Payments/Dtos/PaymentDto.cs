using StudentManagmentSystemApi.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.Payments.Dtos;

public class PaymentDto
{

    [Range(0, 500)]
    public int price { get; set; }
    public string StudentId { get; set; } = default!;
    public string MonthId { get; set; } = default!;
  

}
