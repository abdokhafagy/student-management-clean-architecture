using AutoMapper;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Application.Payments.Dtos;

internal class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, GetPaymentDto>();
        CreateMap<PaymentDto, Payment>();
    }
}
