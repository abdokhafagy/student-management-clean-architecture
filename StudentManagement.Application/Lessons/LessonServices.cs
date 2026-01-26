using AutoMapper;
using StudentManagement.Application.Lessons.Dtos;
using StudentManagement.Application.Response;
using StudentManagement.Domain.Helper;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagmentSystemApi.Data.Entities;
using System.Security.Cryptography;


namespace StudentManagement.Application.Lessons;
internal class LessonServices(IBaseRepository<Lesson> _repoLesson,
    IBaseRepository<Month> _repoMonth, 
    IBaseRepository<Group> _repoGroup, 
    IBaseRepository<Student> _repoStudent,
    IBaseRepository<Attendance> _repoAttendance,
    IMapper mapper) : ILessonServices
{
    public async Task<ResponseDataModel<IEnumerable<GetLessonDto>>> GetAll(string? monthId, string? groupId)
    {
        try
        {
            // Step 1: Validate if the provided monthId exists in the database
            if (!string.IsNullOrEmpty(monthId) && !await _repoMonth.ExistsAsync(m => m.Id == monthId))
            {
                return new ResponseDataModel<IEnumerable<GetLessonDto>>
                {
                    IsSuccess = false,
                    message = "The provided month ID does not exist.",
                    data = null
                };
            }

            // Step 2: Validate if the provided groupId exists in the database
            if (!string.IsNullOrEmpty(groupId) && !await _repoLesson.ExistsAsync(l => l.GroupId == groupId))
            {
                return new ResponseDataModel<IEnumerable<GetLessonDto>>
                {
                    IsSuccess = false,
                    message = "The provided group ID does not exist.",
                    data = null
                };
            }

            // Step 3: Fetch lessons based on the provided filters
            var lessons = await _repoLesson.GetAllAsync(
                l => (string.IsNullOrEmpty(monthId) || l.MonthId == monthId) &&
                     (string.IsNullOrEmpty(groupId) || l.GroupId == groupId));

            // Step 4: Map lessons to DTOs
            var lessonsDtos = mapper.Map<IEnumerable<GetLessonDto>>(lessons);

            // Step 5: Return successful response with data
            return new ResponseDataModel<IEnumerable<GetLessonDto>>
            {
                IsSuccess = true,
                message = "Lessons retrieved successfully.",
                data = lessonsDtos
            };
        }
        catch (Exception ex)
        {
            // Handle exceptions and return failure response
            return new ResponseDataModel<IEnumerable<GetLessonDto>>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseDataModel<GetLessonDto>> GetByIdAsync(string id)
    {
        try
        {
            // Retrieve the student from the repository
            var student = await _repoLesson.GetByIdAsync(id);

            // Check if the student exists
            if (student == null)
            {
                return new ResponseDataModel<GetLessonDto>
                {
                    IsSuccess = false,
                    message = "Lesson not found",
                    data = null
                };
            }

            // Map the student to GetStudentDto
            var lessonDto = mapper.Map<GetLessonDto>(student);

            // Return a success response with the student data
            return new ResponseDataModel<GetLessonDto>
            {
                IsSuccess = true,
                message = "Lesson retrieved successfully",
                data = lessonDto
            };
        }
        catch (Exception ex)
        {
            // Return an error response in case of an exception
            return new ResponseDataModel<GetLessonDto>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseIdModel> AddAsync(LessonDto model)
    {
        try
        {
            //check date of lesson
            if (model.EndDate <= LocalDate.GetLocalDate())
                return new ResponseIdModel { IsSuccess = false, message = $"can't update lesson with date in past !" };

            // Step 1: Get or create the current month
            var currentMonth = (await _repoMonth.OrderByAsync(m => m.CreatedAt, null!, true, 1)).FirstOrDefault();


            // if month is not found create month 
            if (currentMonth is null)
            {
                currentMonth = new Month
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = LocalDate.GetLocalDate().ToString("MMMM"), // Dynamically set the current month name
                    CreatedAt = LocalDate.GetLocalDate()
                };
                await _repoMonth.AddAsync(currentMonth);
            }


            // Step 2: Validate the group ID
            if (!await _repoGroup.ExistsAsync(l => l.Id == model.GroupId))
            {
                return new ResponseIdModel { IsSuccess = false, message = "The group ID is not valid!" };
            }

            // Step 3: Check for an active lesson in the current month
            var existingLesson = (await _repoLesson
                .OrderByAsync(l => l.StartDate, l => l.GroupId == model.GroupId && l.MonthId == currentMonth.Id, true, 1))
                .FirstOrDefault();

            if (existingLesson is not null && !existingLesson.IsEnded)
            {
                return new ResponseIdModel { IsSuccess = true, message = "This lesson already exists. You can take attendance for it.", Id = existingLesson.Id };
            }

            // Step 4: If the lesson count exceeds 8, create a new month
            var lessonCountInMonth = await _repoLesson.CountAsync(l => l.MonthId == currentMonth.Id && l.GroupId == model.GroupId);
            if (lessonCountInMonth >= 8)
            {
                var nextMonthName = LocalDate.GetLocalDate().AddMonths(1).ToString("MMMM");  // Get next month's name
              
                currentMonth = new Month
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nextMonthName,
                    CreatedAt = LocalDate.GetLocalDate(),
                };
                await _repoMonth.AddAsync(currentMonth);
            }

            // Step 5: Create the new lesson
            var lesson = mapper.Map<Lesson>(model);

            lesson.Id = Guid.NewGuid().ToString();
            lesson.StartDate = LocalDate.GetLocalDate();
            lesson.MonthId = currentMonth.Id;
            Console.WriteLine(LocalDate.GetLocalDate());

            // Step 6: Add the new lesson to the database
            await _repoLesson.AddAsync(lesson);
            return new ResponseIdModel { IsSuccess = true, message = "The lesson was successfully added.", Id = lesson.Id };
        }
        catch (Exception ex)
        {
            // Log exception here (optional)
            return new ResponseIdModel { IsSuccess = false, message = $"An error occurred: {ex.Message}" };
        }
    }
    public async Task<ResponseIdModel> FinishAsync(string lessonId)
    {
        // Step 1: Get the lesson by ID
        var lesson = await _repoLesson.GetByIdAsync(lessonId);
        if (lesson is null)
            return new ResponseIdModel { IsSuccess = false, message = $"No lesson found with ID: {lessonId}" };

        //  who don't have attendance for this lesson

        // Step 1: Get all active students in the group
        var students = await _repoStudent.WhereAsync(s => s.GroupId == lesson.GroupId && s.IsActive);

        // Step 2: Filter students who don't have attendance for the current lesson
        var studentsWithoutAttendance = new List<Student>();

        foreach (var student in students)
        {
            bool hasAttendance = await _repoAttendance.ExistsAsync(a => a.LessonId == lessonId && a.StudentId == student.Id);
            if (!hasAttendance)
            {
                studentsWithoutAttendance.Add(student);
            }
        }


        // Check if there are no students without attendance
        if (!studentsWithoutAttendance.Any())
        {
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = $"All students in the group already have attendance for lesson ID: {lessonId}",
                Id = lesson.Id
            };
        }

        // Step 3: Add attendance records for students without attendance
        var newAttendances = studentsWithoutAttendance.Select(student => new Attendance
        {
            Id = Guid.NewGuid().ToString(),
            LessonId = lessonId,
            StudentId = student.Id,
            AttendanceStatus = false, // Set the appropriate status
            CreatedAt = LocalDate.GetLocalDate(),
        }).ToList();

        try
        {
            // Save the new attendance records to the database
            await _repoAttendance.AddRangeAsync(newAttendances);

            await _repoLesson.UpdateAsync(lesson);

            return new ResponseIdModel
            {
                IsSuccess = true,
                message = $"Lesson with ID: {lessonId} successfully finished. Attendance marked for all students.",
                Id = lessonId
            };
        }
        catch (Exception ex)
        {
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"An error occurred while finishing the lesson: {ex.Message}"
            };
        }
    }

    public async Task<ResponseIdModel> UpdateAsync(string id, UpdateLessonDto model)
    {
        try
        {
            // Find the existing lesson by ID
            var lesson = await _repoLesson.GetByIdAsync(id);
            if (lesson == null)
                return new ResponseIdModel { IsSuccess = false, message = $"Lesson with ID '{id}' not found." };
            
            //check date of lesson
            if(model.EndDate <= LocalDate.GetLocalDate())
                return new ResponseIdModel { IsSuccess = false, message = $"can't update lesson with date in past !" };

            // Update only the EndDate field from the DTO
            lesson.EndDate = model.EndDate;

            // Update the entity in the repository
            await _repoLesson.UpdateAsync(lesson);

            // Return success response
            return new ResponseIdModel { IsSuccess = true, message = "Lesson EndDate was successfully updated.",Id=lesson.Id };
        }
        catch (Exception ex)
        {
            // Return error response
            return new ResponseIdModel { IsSuccess = false, message = $"An error occurred: {ex.Message}" };
        }
    }
    public async Task DeleteAsync(string id)
    {
        await _repoLesson.DeleteAsync(id);
    }
    public async Task<int> CountAsync()
    {
        return await _repoLesson.CountAsync();
    }

}

