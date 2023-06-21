using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data;

public class Context : IdentityDbContext<Employee>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Response> Responses => Set<Response>();
    public DbSet<Contract> Contracts => Set<Contract>();

    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Request>()
            .HasOne(r => r.Response)
            .WithOne(r => r.Request)
            .HasForeignKey<Response>(r => r.RequestID);

        modelBuilder.Entity<Employee>()
            .ToTable($"{nameof(Employee)}s");

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(e => e.DeleteDate == null);

        modelBuilder.Entity<Request>()
            .HasQueryFilter(r => r.CreatedBy.DeleteDate == null);

        modelBuilder.Entity<Response>()
            .HasQueryFilter(r => r.Request.CreatedBy.DeleteDate == null);

        modelBuilder.Entity<Contract>()
            .HasQueryFilter(c => c.Employee.DeleteDate == null);

        modelBuilder.Entity<Response>()
            .HasOne(v => v.ReviewedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        DataSeeder.SeedRootUser(modelBuilder);

        DataSeeder.SeedLeaveType(modelBuilder, 1, "Sick Leave", "Neki opis");
        DataSeeder.SeedLeaveType(modelBuilder, 2, "Days off", "Neki opis");
        DataSeeder.SeedLeaveType(modelBuilder, 3, "Paid leave", "Neki opis");
        DataSeeder.SeedLeaveType(modelBuilder, 4, "Unpaid leave", "Neki opis");

        DataSeeder.SeedPosition(modelBuilder, 2, "Software Engineer", "Responsible for developing software applications.");
        DataSeeder.SeedPosition(modelBuilder, 3, "Project Manager", "Leading project teams and ensuring project success.");
    }
}
