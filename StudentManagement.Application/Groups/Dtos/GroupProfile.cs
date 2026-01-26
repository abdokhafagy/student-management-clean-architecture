using AutoMapper;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Groups.Dtos;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<GroupDto, Group>();
        CreateMap<Group, GetGroupDto>();
    }
}
