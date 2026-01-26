using Microsoft.EntityFrameworkCore;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagmentSystemApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Month> Months { get; set; }
    public DbSet<Payment> Payments { get; set; }
    // public DbSet<Expenses> Expenses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // var userId = Guid.NewGuid().ToString();

        //seed data 
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Id)
        //    .HasDefaultValueSql("NewID()");

        //modelBuilder.Entity<User>()
        //    .HasData(
        //    new User() { Id = userId, Name = "admin", Phone = "111111111", Role = UserRole.Role.Admin, CreatedAt = DateTime.UtcNow, }
        //    );
    }
}
