

namespace StudentManagement.Application.Lessons.Dtos;

public class GetLessonDto
{
    public string Id { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsEnded { get; set; }
    public string GroupId { get; set; } = default!;
    public string MonthId { get; set; } = default!;
}
