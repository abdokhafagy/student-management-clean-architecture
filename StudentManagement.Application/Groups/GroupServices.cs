using AutoMapper;
using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;
using StudentManagement.Domain.Helper;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagmentSystemApi.Data.Entities;


namespace StudentManagement.Application.Groups;

public class GroupServices(IBaseRepository<Group> _repoGroup, IMapper mapper) : IGroupServices
{
    public async  Task<ResponseDataModel<IEnumerable<GetGroupDto>>> GetAll(byte? year)
    {
        try
        {
            var groups = year == null
            ? await _repoGroup.GetAllAsync()
            : await _repoGroup.GetAllAsync(gr => gr.AcademicYear == year);
            var groupsDtos = mapper.Map<IEnumerable<GetGroupDto>>(groups);
            return new ResponseDataModel<IEnumerable<GetGroupDto>>
            {
                IsSuccess = true,
                message = "Groups retrieved successfully",
                data = groupsDtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<IEnumerable<GetGroupDto>>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseDataModel<GetGroupDto>> GetByIdAsync(string id)
    {
        try
        {
            var group = await _repoGroup.GetByIdAsync(id);
            var groupDto = mapper.Map<GetGroupDto>(group);
            return new ResponseDataModel<GetGroupDto>
            {
                IsSuccess = true,
                message = "Students retrieved successfully",
                data = groupDto
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<GetGroupDto>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseIdModel> AddAsync(GroupDto model)
    {
        // valdation on the group ,  name year , lessondate 
        if (await _repoGroup.ExistsAsync(g => g.Name == model.GroupName && g.LessonDate == model.LessonDate))
            return new ResponseIdModel { IsSuccess = false, message = " the group is already exist !" };

        var group = mapper.Map<Group>(model);

        group.Id = Guid.NewGuid().ToString();
        group.CreatedAt =LocalDate.GetLocalDate();

        await _repoGroup.AddAsync(group);

        return new ResponseIdModel { IsSuccess = true, message = "Created success", Id = group.Id };
    }
    public async Task<ResponseIdModel>  UpdateAsync(string id, GroupDto model)
    {
        // Find the existing group by ID
        var group = await _repoGroup.GetByIdAsync(id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with ID '{id}' not found.");
        }
        if (await _repoGroup.ExistsAsync(g => g.Name == model.GroupName && g.LessonDate == model.LessonDate && group.Id != id))
            return new ResponseIdModel { IsSuccess = false, message = " the group is already exist !" };

        // Map updated values from the DTO to the entity
        mapper.Map(model, group);
        
        // Update the entity in the repository
        await _repoGroup.UpdateAsync(group);
        return new ResponseIdModel { IsSuccess = true, message = "Created success", Id = group.Id };
    }
    public async Task<int> DeleteAsync(string id)
    {
        return await _repoGroup.DeleteAsync(id);
    }
    public async Task<int> CountAsync(byte? year)
    {
        return await _repoGroup.CountAsync(group => (!year.HasValue) || group.AcademicYear == year);
    }
}
