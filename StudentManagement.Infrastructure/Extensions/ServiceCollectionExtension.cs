using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Domain.IBaseRepositories;

using StudentManagement.Infrastructure.ImpRepositories;
using StudentManagement.Infrastructure.Seeder;
using StudentManagmentSystemApi.Data;

namespace StudentManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configration)
    {
        var connection = configration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connection));

        services.AddScoped<IUserSeeder, UserSeeder>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        //services.AddScoped<IStudentRepository,StudentRepository>();
        //services.AddScoped<IGroupRepository,GroupRepository>();


    }
}
