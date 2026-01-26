using StudentManagement.Application.Payments.Dtos;
using StudentManagement.Application.Response;


namespace StudentManagement.Application.Payments;

public interface IPaymentServices
{
    Task<ResponseDataModel<IEnumerable<GetPaymentDto>>> GetAllAsync(string? studentId);
    Task<ResponseDataModel<GetPaymentDto>> GetByIdAsync(string id);
    Task<ResponseIdModel> AddAsync(PaymentDto model);
    Task<ResponseIdModel> UpdateAsync(string id, bool IsPaid);
    Task<int> DeleteAsync(string id);
}
