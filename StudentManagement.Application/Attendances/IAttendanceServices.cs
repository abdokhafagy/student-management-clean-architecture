using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;

namespace StudentManagement.Application.Attendances;

public interface IAttendanceServices
{
    Task<ResponseDataModel<IEnumerable<GetAttendanceDto>>> GetAll(string? studentId);
    Task<ResponseDataModel<GetAttendanceDto>> GetByIdAsync(string id);
    Task<ResponseIdModel> AddAsync(AttendanceDto model);
    Task<ResponseIdModel> UpdateAsync(string id, bool AttendaceStatus);
    Task DeleteAsync(string id);
}