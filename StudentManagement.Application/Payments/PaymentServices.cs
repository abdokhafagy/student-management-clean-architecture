

using AutoMapper;
using StudentManagement.Application.Payments.Dtos;
using StudentManagement.Application.Response;
using StudentManagement.Application.Students.Dtos;
using StudentManagement.Domain.Helper;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Payments;

internal class PaymentServices(IBaseRepository<Payment> _repoPayment ,
    IBaseRepository<Student> _repoStudent ,
    IBaseRepository<Month> _repoMonth,
    IMapper mapper) :IPaymentServices 
{
    public async Task<ResponseDataModel<IEnumerable<GetPaymentDto>>> GetAllAsync(string? studentId)
    {
        try
        {
            var payments = await _repoPayment.GetAllAsync(stud => string.IsNullOrEmpty(studentId) || stud.StudentId == studentId);
            var paymentsDtos = mapper.Map<IEnumerable<GetPaymentDto>>(payments);

            return new ResponseDataModel<IEnumerable<GetPaymentDto>>
            {
                IsSuccess = true,
                message = "Payments retrieved successfully",
                data = paymentsDtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<IEnumerable<GetPaymentDto>>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseDataModel<GetPaymentDto>> GetByIdAsync(string id)
    {
        try
        {
            // Retrieve the student from the repository
            var payment = await _repoPayment.GetByIdAsync(id);

            // Check if the student exists
            if (payment == null)
            {
                return new ResponseDataModel<GetPaymentDto>
                {
                    IsSuccess = false,
                    message = "Payment not found",
                    data = null
                };
            }

            // Map the student to GetStudentDto
            var studentDto = mapper.Map<GetPaymentDto>(payment);

            // Return a success response with the student data
            return new ResponseDataModel<GetPaymentDto>
            {
                IsSuccess = true,
                message = "Payment retrieved successfully",
                data = studentDto
            };
        }
        catch (Exception ex)
        {
            // Return an error response in case of an exception
            return new ResponseDataModel<GetPaymentDto>
            {
                IsSuccess = false,
                message = $"Error occurred: {ex.Message}",
                data = null
            };
        }
    }
    public async Task<ResponseIdModel> AddAsync(PaymentDto model)
    {
        try
        {
            // not make create if lesson is eneded and student not active 

            // Check if student Id is found 
            if (!await _repoStudent.ExistsAsync(s => s.Id == model.StudentId && s.IsActive && !s.IsDeleted))
                return new ResponseIdModel { IsSuccess = false, message = "student is not found !" };

            // Check if month Id is found 
            if (!await _repoMonth.ExistsAsync(s => s.Id == model.MonthId))
                return new ResponseIdModel { IsSuccess = false, message = "Month is not found  !" };
            

            // Check if payment for the student in this lesson already exists
            var existingPayment = await _repoPayment.SingleOrDefaultAsync(a => a.StudentId == model.StudentId && a.MonthId == model.MonthId);
            if (existingPayment is not null)
            {
                if(existingPayment.IsPaid is false){
                    existingPayment.IsPaid = true;
                    await _repoPayment.UpdateAsync(existingPayment);
                }
                return new ResponseIdModel { IsSuccess = true, message = "Payment for this student in this month already exists. and make it true .",Id=existingPayment.Id };
            }
            // Map the DTO to the payment entity
            var payment = mapper.Map<Payment>(model);
            payment.Id = Guid.NewGuid().ToString(); // Generate a new ID for the attendance
            payment.IsPaid = true;
            payment.CreatedAt = LocalDate.GetLocalDate();

            // Add the new payment record to the repository
            await _repoPayment.AddAsync(payment);

            // Return a response model with success status and the new attendance ID
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = "Payment successfully added.",
                Id = payment.Id
            };
        }
        catch (Exception ex)
        {
            // Handle any errors and return a meaningful message
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"An error occurred while adding payment: {ex.Message}"
            };
        }
    }
    public async Task<ResponseIdModel> UpdateAsync(string id,bool IsPaid)
    {
        try
        {
            // Step 1: Find the existing payment record by ID
            var payment = await _repoPayment.GetByIdAsync(id);
            if (payment is null)
                return new ResponseIdModel { IsSuccess = false, message = $"Payment with ID '{id}' not found." };

            // Step 2: Map updated values from the DTO to the entity
            payment.IsPaid = IsPaid;
            payment.UpdatedAt = LocalDate.GetLocalDate();
            // Step 3: Update the entity in the repository
            await _repoPayment.UpdateAsync(payment);

            // Step 4: Return success response
            return new ResponseIdModel
            {
                IsSuccess = true,
                message = "payment successfully updated.",
                Id = id
            };
        }
        catch (Exception ex)
        {
            // Handle any errors and return an appropriate message
            return new ResponseIdModel
            {
                IsSuccess = false,
                message = $"An error occurred while updating payment: {ex.Message}"
            };
        }
    }
    public async Task<int> DeleteAsync(string id)
    {
        return await _repoPayment.DeleteAsync(id);
    }
}
