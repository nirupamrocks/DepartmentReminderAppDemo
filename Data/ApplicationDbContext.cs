using Microsoft.EntityFrameworkCore;
using DepartmentReminderAppDemo.Models;


namespace DepartmentReminderAppDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Department Entity Configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Logo)
                    .HasColumnType("VARBINARY(MAX)");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.ParentDepartment)
                    .WithMany(p => p.SubDepartments)
                    .HasForeignKey(d => d.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.NoAction); // To avoid cascade delete issues

                entity.HasIndex(e => e.ParentDepartmentId)
                    .HasDatabaseName("IDX_Departments_ParentDepartmentId");

                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("IDX_Departments_Name");
            });

            // Reminder Entity Configuration
            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DueDate)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.IsNotified)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(r => r.Department)
                    .WithMany()
                    .HasForeignKey(r => r.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull); // Optional FK, set to null if department is deleted

                entity.HasIndex(e => e.DepartmentId)
                    .HasDatabaseName("IDX_Reminders_DepartmentId");

                entity.HasIndex(e => e.DueDate)
                    .HasDatabaseName("IDX_Reminders_DueDate");

                entity.HasIndex(e => e.Title)
                    .HasDatabaseName("IDX_Reminders_Title");
            });
        }
    }
}
