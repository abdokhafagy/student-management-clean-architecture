using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Response;

namespace StudentManagement.Application.Groups;

public interface IGroupServices
{
    Task<ResponseDataModel<IEnumerable<GetGroupDto>>> GetAll(byte? year);
    Task<ResponseDataModel<GetGroupDto>> GetByIdAsync(string id);
    Task<ResponseIdModel> AddAsync(GroupDto model);
    Task<ResponseIdModel> UpdateAsync(string id ,GroupDto model);
    Task<int> DeleteAsync(string id);
    Task<int> CountAsync(byte? year);
}