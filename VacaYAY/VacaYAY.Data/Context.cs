using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

    private readonly IConfiguration _config;
    public Context(
        DbContextOptions<Context> options,
        IConfiguration config) : base(options)
    {
        _config = config;
    }

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
        DataSeeder.SeedRootUser(modelBuilder, _config);
        DataSeeder.SeedPositions(modelBuilder, _config);
        DataSeeder.SeedLeaveTypes(modelBuilder, _config);
    }
}
