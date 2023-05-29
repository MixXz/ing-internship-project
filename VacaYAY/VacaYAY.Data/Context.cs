using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data;

public class Context : IdentityDbContext<Employee>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Response> Responses => Set<Response>();

    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Request>()
            .HasOne(r => r.Response)
            .WithOne(r => r.Request)
            .HasForeignKey<Response>(r => r.RequstID);

        modelBuilder.Entity<Employee>().ToTable("Employes");
    }
}
