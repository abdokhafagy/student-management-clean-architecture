
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.Students.Dtos;

public class StudentDto
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public string Phone { get; set; } = default!;
    [Required]
    public string ParentPhone { get; set; } = default!;
    [Required] 
    public string GroupId { get; set; } = default!;

}
