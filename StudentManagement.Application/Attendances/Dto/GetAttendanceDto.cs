using StudentManagmentSystemApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Attendances.Dto
{
    public class GetAttendanceDto
    {
        public string Id { get; set; } = default!;
        public bool AttendanceStatus { get; set; }

        public string StudentId { get; set; } = default!;

        public string LessonId { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
