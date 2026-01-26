using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Users
{
    public interface IStudentServices
    {
        Task<ResponseDataModel<IEnumerable<GetStudentDto>>> GetAllAsync(string? groupId);
        Task<ResponseDataModel<GetStudentDto>> GetByIdAsync(string id);
        Task<ResponseIdModel> AddAsync(StudentDto model);
        Task<ResponseIdModel> ActiveAsync(string studentId);
        Task<ResponseIdModel> UpdateAsync(string id, StudentDto model);
        Task<int> DeleteAsync(string id);
        Task<int> CountAsync(string? groupId ,byte? year);
    }
}