using AutoMapper;
using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;
using StudentManagmentSystemApi.Data.Entities;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagement.Domain.Helper;
using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Payments.Dtos;
namespace StudentManagement.Application.Users;


public class StudentServices(IBaseRepository<Student> _repoStudent,
                        IBaseRepository<Group> _repoGroup, 
                        IBaseRepository<Attendance> _repoAttendance,
                        IBaseRepository<Payment> _repoPayment,
                        IMapper mapper) : IStudentServices
{
    public async Task<ResponseDataModel<IEnumerable<GetStudentDto>>> GetAllAsync(string? groupId)
    {
        try
        {
            // var students = await _repoStudent.GetAll(stud=> string.IsNullOrEmpty(groupId) || stud.GroupId == groupId,s=>s.Attendances);
            var students = await _repoStudent.GetAllAsync<Student, GetStudentDto>(
                                    predicate: stud => string.IsNullOrEmpty(groupId) || stud.GroupId == groupId,
                                    selector: student => new GetStudentDto
                                    {
                                        Id = student.Id,
                                        Name = student.Name,
                                        Phone = student.Phone,
                                        ParentPhone = student.ParentPhone,
                                        Comments = student.Comments!,
                                        IsDeleted = student.IsDeleted,
                                        IsActive = student.IsActive,
                                        CreatedAt = student.CreatedAt,
                                        UpdatedAt = student.UpdatedAt,
                                        GroupId = student.GroupId,
                                        Attendances = new List<GetAttStudentDto>(), 
                                        Payments = new List<GetPayStudentDto>()
                                    },
                                    includes: s => s.Attendances
                                );

            // Now populate Attendances separately
            foreach (var student in students)
            {
                // Fetch attendances for a student, ordering them as needed
                var attendances = _repoAttendance.OrderBy(
                    orderBy: att => att.CreatedAt,
                    filter: att => att.StudentId == student.Id, // Adjust filter based on your model
                    descending: true,
                    take: 2
                ).ToList();

                var mappedAttendances = mapper.Map<List<GetAttStudentDto>>(attendances);
                student.Attendances = mappedAttendances;

                // Fetch attendances for a student, ordering them as needed
                var payments = _repoPayment.OrderBy(
                    orderBy: att => att.CreatedAt,
                    filter: att => att.StudentId == student.Id, // Adjust filter based on your model
                    descending: true,
                    take: 2
                ).ToList();

                var mappedPayments = mapper.Map<List<GetPayStudentDto>>(payments);
                student.Payments = mappedPayments;

            }


            var studentDtos = mapper.Map<IEnumerable<GetStudentDto>>(students);

            return new ResponseDataModel<IEnumerable<GetStudentDto>>
            {
                IsSuccess = true,
                message = "Students retrieved successfully",
                data = studentDtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<IEnumerable<GetStudentDto>>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseDataModel<GetStudentDto>> GetByIdAsync(string id)
    {
        try
        {
            // Retrieve the student from the repository
            var student = await _repoStudent.GetByIdAsync(id);

            // Check if the student exists
            if (student == null)
            {
                return new ResponseDataModel<GetStudentDto>
                {
                    IsSuccess = false,
                    message = "Student not found",
                    data = null
                };
            }

            // Map the student to GetStudentDto
            var studentDto = mapper.Map<GetStudentDto>(student);

            // Return a success response with the student data
            return new ResponseDataModel<GetStudentDto>
            {
                IsSuccess = true,
                message = "Student retrieved successfully",
                data = studentDto
            };
        }
        catch (Exception ex)
        {
            // Return an error response in case of an exception
            return new ResponseDataModel<GetStudentDto>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseIdModel> AddAsync(StudentDto model)
    {
        // make valdateion for  phone 
        if (model.Phone.Length != 11 && model.ParentPhone.Length != 11 )
            return new ResponseIdModel { IsSuccess = false, message = " Phone or ParentPhone should be 11 digit" };

        // check the group id has exists ;
        if (!await _repoGroup.ExistsAsync(g=>g.Id == model.GroupId))
            return new ResponseIdModel { IsSuccess = false, message = " the group id is not valid !" };

        // check the name  has exists ;
        if (await _repoStudent.ExistsAsync(stud => stud.Name == model.Name))
            return new ResponseIdModel { IsSuccess = false, message = " the name is already exists before !" };

        var student = mapper.Map<Student>(model);

        student.Id = Guid.NewGuid().ToString();
        student.CreatedAt = LocalDate.GetLocalDate();

        await _repoStudent.AddAsync(student);
        return new ResponseIdModel { IsSuccess = true, message = "Created success", Id = student.Id };

    }
    public async Task<ResponseIdModel> ActiveAsync(string studentId)
    {
        try
        {
            // Retrieve the student from the repository
            var student = await _repoStudent.GetByIdAsync(studentId);

            // Check if the student exists
            if (student == null)
            {
                return new ResponseIdModel
                {
                    IsSuccess = false,
                    message = "Student not found",
                };
            }

            // Map the student to GetStudentDto
             student.IsActive = true;
            student.UpdatedAt = LocalDate.GetLocalDate();
            await _repoStudent.UpdateAsync(student);
            // Return a success response with the student data
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = "Student Actived successfully",
                Id = studentId
                
            };
        }
        catch (Exception ex)
        {
            // Return an error response in case of an exception
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
            };
        }
    }

    public async Task<ResponseIdModel> UpdateAsync(string id, StudentDto model)
    {
        // Validate phone numbers 
        if (model.Phone.Length != 11 && model.ParentPhone.Length != 11)
            return new ResponseIdModel { IsSuccess = false, message = "Phone or ParentPhone should be 11 digits" };

        // Fetch the student 
        var student = await _repoStudent.GetByIdAsync(id);
        if (student == null)
            return new ResponseIdModel { IsSuccess = false, message = $"User with id: {id} is not found" };

        // Validate group is exist
        if (!await _repoGroup.ExistsAsync(g => g.Id == model.GroupId))
            return new ResponseIdModel { IsSuccess = false, message = "The group ID is not valid!" };

        // Check for duplicate name, excluding the current student
        if (await _repoStudent.ExistsAsync(stud => stud.Name == model.Name && stud.Id != id))
            return new ResponseIdModel { IsSuccess = false, message = "The name already exists!" };

        // Map updated values and update the entity
        mapper.Map(model, student);
        student.UpdatedAt = LocalDate.GetLocalDate();
        await _repoStudent.UpdateAsync(student);

        return new ResponseIdModel { IsSuccess = true, message = "Update successful", Id = student.Id };
    }

    public async Task<int> DeleteAsync(string id)
    {
     return  await _repoStudent.DeleteAsync(id);
    }
    public async Task<int> CountAsync(string? groupId, byte? year)
    {
        return await _repoStudent.CountAsync(
            g => (!string.IsNullOrEmpty(groupId) && g.Group.Id == groupId) &&  // and  ignore with false . 
                 (!year.HasValue || g.Group.AcademicYear == year),   // or ignore with true .
            g => g.Group
        );
    }


}
