using AutoMapper;
using StudentManagmentSystemApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Attendances.Dto
{
    internal class AttendanceProfile :Profile
    {
        public AttendanceProfile()
        {
            CreateMap<AttendanceDto, Attendance>();
            CreateMap<Attendance, GetAttendanceDto>();


        }
    }
}
