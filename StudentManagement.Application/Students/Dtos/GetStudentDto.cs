using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Payments.Dtos;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Students.Dtos;

public class GetStudentDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string ParentPhone { get; set; } = default!;
    public string Comments { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string GroupId { get; set; } = default!;
    public List<GetAttStudentDto> Attendances { get; set; } = default!;
    public List<GetPayStudentDto> Payments { get; set; } = default!;


}
