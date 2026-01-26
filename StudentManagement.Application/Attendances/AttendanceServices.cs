
using AutoMapper;
using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Response;
using StudentManagement.Domain.Helper;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Attendances;

internal class AttendanceService(IBaseRepository<Attendance> _repoAttendance
    ,IBaseRepository<Student> _repoStudent
    ,IBaseRepository<Lesson> _repoLesson
    , IMapper mapper):IAttendanceServices
{
    // Add , Udate only make .
    public async Task<ResponseDataModel<IEnumerable<GetAttendanceDto>>> GetAll(string? studentId)
    {
        
        try
        {
            var attendances = await _repoAttendance.GetAllAsync(stud => string.IsNullOrEmpty(studentId) || stud.StudentId == studentId);


        var attendanceDtos = mapper.Map<IEnumerable<GetAttendanceDto>>(attendances);

            return new ResponseDataModel<IEnumerable<GetAttendanceDto>>
            {
                IsSuccess = true,
                message = "Attendances retrieved successfully",
                data = attendanceDtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<IEnumerable<GetAttendanceDto>>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
  
    public async Task<ResponseDataModel<GetAttendanceDto>> GetByIdAsync(string id)
    {

        try
        {
            var attendance = await _repoAttendance.GetByIdAsync(id);
            // Check if the student exists
            if (attendance == null)
            {
                return new ResponseDataModel<GetAttendanceDto>
                {
                    IsSuccess = false,
                    message = "Attendance not found",
                    data = null
                };
            }


            var attendanceDto = mapper.Map<GetAttendanceDto>(attendance);

            return new ResponseDataModel<GetAttendanceDto>
            {
                IsSuccess = true,
                message = "Attendances retrieved successfully",
                data = attendanceDto
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<GetAttendanceDto>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
     
    public async Task<ResponseIdModel> AddAsync(AttendanceDto model)
    {
        try
        {
            // not make create if lesson is eneded and student not active 

            // Check if student Id is found 
            if(!await _repoStudent.ExistsAsync(s=>s.Id == model.StudentId && s.IsActive && !s.IsDeleted))
                return new ResponseIdModel{ IsSuccess = false,message = "student is not found !" };

            // Check if Lesson Id is found 
            var lesson = await _repoLesson.GetByIdAsync(model.LessonId);
            if(lesson is null || lesson.IsEnded)
            {
                return new ResponseIdModel { IsSuccess = false, message = "Lesson is not found or ended !" };
            }
           
            // Check if attendance for the student in this lesson already exists
            var existingAttendance = await _repoAttendance.SingleOrDefaultAsync(a => a.StudentId == model.StudentId && a.LessonId == model.LessonId);
            if (existingAttendance is not null)
            {
                if (existingAttendance.AttendanceStatus is false)
                {
                    existingAttendance.AttendanceStatus = true;
                    await _repoAttendance.UpdateAsync(existingAttendance);
                }
                return new ResponseIdModel { IsSuccess = true, message = "Attendance for this student in this lesson already exists. and make it true .", Id = existingAttendance.Id };
            }
            // Map the DTO to the Attendance entity
            var attendance = mapper.Map<Attendance>(model);
            attendance.Id = Guid.NewGuid().ToString(); // Generate a new ID for the attendance
            attendance.AttendanceStatus = true;
            attendance.CreatedAt = LocalDate.GetLocalDate();

            // Add the new attendance record to the repository
            await _repoAttendance.AddAsync(attendance);

            // Return a response model with success status and the new attendance ID
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = "Attendance successfully added.",
                Id = attendance.Id
            };
        }
        catch (Exception ex)
        {
            // Handle any errors and return a meaningful message
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"An error occurred while adding attendance: {ex.Message}"
            };
        }
    }
  
    public async Task<ResponseIdModel> UpdateAsync(string id, bool AttendaceStatus)
    {
        try
        {
            // Step 1: Find the existing attendance record by ID
            var attendance = await _repoAttendance.GetByIdAsync(id);
            if (attendance is null)
                return new ResponseIdModel { IsSuccess = false, message = $"Attendance with ID '{id}' not found." };

            // Step 2: Map updated values from the DTO to the entity
            attendance.AttendanceStatus = AttendaceStatus;
            attendance.UpdatedAt = LocalDate.GetLocalDate();
            // Step 3: Update the entity in the repository
            await _repoAttendance.UpdateAsync(attendance);

            // Step 4: Return success response
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = "Attendance successfully updated.",
                Id = id
            };
        }
        catch (Exception ex)
        {
            // Handle any errors and return an appropriate message
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"An error occurred while updating attendance: {ex.Message}"
            };
        }
    }

    public async Task DeleteAsync(string id)
    {
        await _repoAttendance.DeleteAsync(id);
    }

}
