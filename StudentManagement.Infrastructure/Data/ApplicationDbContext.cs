using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagmentSystemApi.Data.Entities;

namespace StudentManagmentSystemApi.Data;

public class ApplicationDbContext : IdentityDbContext<User>
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
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // var userId = Guid.NewGuid().ToString();

    //    //seed data 
    //    //modelBuilder.Entity<User>()
    //    //    .Property(u => u.Id)
    //    //    .HasDefaultValueSql("NewID()");

    //    //modelBuilder.Entity<User>()
    //    //    .HasData(
    //    //    new User() { Id = userId, Name = "admin", Phone = "111111111", Role = UserRole.Role.Admin, CreatedAt = DateTime.UtcNow, }
    //    //    );
    //}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships with RESTRICT delete behavior
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
        {
            // Configure CreatedBy relationship (RESTRICT)
            modelBuilder.Entity(entityType.ClrType)
                .HasOne(nameof(BaseEntity.Creator))
                .WithMany()
                .HasForeignKey(nameof(BaseEntity.CreatedById))
                .OnDelete(DeleteBehavior.Restrict);  // FIX: Change from Cascade to Restrict

            // Configure UpdatedBy relationship (RESTRICT)
            modelBuilder.Entity(entityType.ClrType)
                .HasOne(nameof(BaseEntity.Updater))
                .WithMany()
                .HasForeignKey(nameof(BaseEntity.UpdatedById))
                .OnDelete(DeleteBehavior.Restrict);  // FIX: Change from Cascade to Restrict
        }

        // Configure specific entities
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(s => s.Phone).IsUnique();

            // Add check constraint
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Student_Phone_NotParentPhone",
                "[Phone] != [ParentPhone]"));
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            // Use decimal for Price
            entity.Property(p => p.Price)
                  .HasColumnType("decimal(10,2)");

            // Unique constraint: one payment per student per month
            entity.HasIndex(p => new { p.StudentId, p.MonthId }).IsUnique();

            // Price must be positive
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Payment_Price_Positive",
                "[Price] > 0"));
        });

        modelBuilder.Entity<Month>(entity =>
        {
            // Unique constraint: month name + year
            entity.HasIndex(m => new { m.Name, m.Year }).IsUnique();
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            // EndDate must be after StartDate
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Lesson_EndDate_After_StartDate",
                "[EndDate] > [StartDate]"));
        });
    }
}

