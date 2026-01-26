using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Payments.Dtos
{
    public class GetPayStudentDto
    {
        public string Id { get; set; } = default!;

        public int price { get; set; }
        public bool IsPaid { get; set; }
        public string MonthId { get; set; } = default!;

    }
}
