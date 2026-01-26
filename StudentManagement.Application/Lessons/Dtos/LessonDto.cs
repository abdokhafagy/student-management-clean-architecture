using StudentManagement.Domain.Helper;
using StudentManagmentSystemApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Lessons.Dtos
{
    public class LessonDto
    {
        public DateTime EndDate { get; set; } = LocalDate.GetLocalDate().AddHours(1);
        public string GroupId { get; set; } = default!;

    }
}
