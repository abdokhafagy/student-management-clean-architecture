using AutoMapper;
using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Payments.Dtos;
using StudentManagmentSystemApi.Data.Entities;


namespace StudentManagement.Application.Students.Dtos;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, GetStudentDto>()
            .ForMember(des=>des.Attendances,src=>src.MapFrom(src=>src.Attendances)); // get 
        CreateMap<StudentDto, Student>();

        CreateMap<Attendance, GetAttStudentDto>();
        CreateMap<Payment, GetPayStudentDto>();
        //   .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone));

    }
}
