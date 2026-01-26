
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Attendances;
using StudentManagement.Application.Groups;
using StudentManagement.Application.Lessons;
using StudentManagement.Application.Payments;
using StudentManagement.Application.Users;

namespace StudentManagement.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IStudentServices, StudentServices>();
        services.AddScoped<IGroupServices, GroupServices>();
        services.AddScoped<ILessonServices, LessonServices>();
        services.AddScoped<IAttendanceServices, AttendanceService>();
        services.AddScoped<IPaymentServices, PaymentServices>();
        services.AddAutoMapper(typeof(ServiceCollectionExtension).Assembly);


    }
}
