using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Lessons.Dtos;
using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;

namespace StudentManagement.Application.Lessons
{
    public interface ILessonServices
    {
        Task<ResponseDataModel<IEnumerable<GetLessonDto>>> GetAll(string? monthId, string? groupId);
        Task<ResponseDataModel<GetLessonDto>> GetByIdAsync(string id);
        Task<ResponseIdModel> AddAsync(LessonDto model);
        Task<ResponseIdModel> FinishAsync(string lessonId);
        Task<ResponseIdModel> UpdateAsync(string id, UpdateLessonDto model);
        Task DeleteAsync(string id);
        Task<int> CountAsync();
    }
}