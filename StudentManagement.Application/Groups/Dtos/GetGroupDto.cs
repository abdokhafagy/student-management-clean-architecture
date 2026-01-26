using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Groups.Dtos
{
    public class GetGroupDto
    {
        public string Id { get; set; } = default!;
        public string GroupName { get; set; } = default!;
        public byte AcademicYear { get; set; }
        public string LessonDate { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
