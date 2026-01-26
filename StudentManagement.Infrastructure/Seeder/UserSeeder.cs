using StudentManagmentSystemApi.Data;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagement.Infrastructure.Seeder;

public class UserSeeder(ApplicationDbContext _context) : IUserSeeder
{
    public async Task Seed()
    {
        if (await _context.Database.CanConnectAsync())
        {
            if (!_context.Students.Any())
            {
                Student user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Phone = "01100312510",
                    Name = "Admin",
                };
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }

        }
    }
}
