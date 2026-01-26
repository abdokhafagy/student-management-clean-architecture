

using StudentManagmentSystemApi.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.Payments.Dtos;

public class GetPaymentDto
{
    public string Id { get; set; } = default!;

    public int price { get; set; }
    public string StudentId { get; set; } = default!;

    public string MonthId { get; set; } = default!;

    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UdatedAt { get; set; }
}
