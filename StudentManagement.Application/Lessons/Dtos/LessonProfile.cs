using AutoMapper;
using StudentManagmentSystemApi.Data.Entities;


namespace StudentManagement.Application.Lessons.Dtos;

internal class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, GetLessonDto>(); // source , distnation 
        CreateMap<LessonDto,Lesson > ();
    }
}
