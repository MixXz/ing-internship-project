using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data;

public class Context : IdentityDbContext<Employee>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Response> Responses => Set<Response>();

    private readonly IConfiguration _config;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    public Context(
        DbContextOptions<Context> options,
        IConfiguration config,
        IPasswordHasher<Employee> passwordHasher) : base(options)
    {
        _config = config;
        _passwordHasher = passwordHasher;
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

        modelBuilder.Entity<Response>()
            .HasOne(v => v.ReviewedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        SeedRootUser(modelBuilder);
    }

    private void SeedRootUser(ModelBuilder builder)
    {
        var userId = Guid.NewGuid().ToString();
        var email = _config["RootUser:Email"]!;
        var placeholder = "Root";

        var hashedPassword = _passwordHasher.HashPassword(new()
        {
            Id = userId,
            Email = email,
        }, _config["RootUser:Password"]!);

        builder.Entity<Position>()
            .HasData(new Position()
            {
                ID = 1,
                Caption = _config["RootPosition:Caption"]!,
                Description = _config["RootPosition:Description"]!
            });

        builder.Entity<Employee>()
            .HasData(new
            {
                Id = userId,
                FirstName = _config["RootUser:FirstName"],
                LastName = _config["RootUser:LastName"],
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                PasswordHash = hashedPassword,
                IDNumber = _config["RootUser:IDNumber"],
                Address = placeholder,
                EmployeeStartDate = DateTime.Now,
                InsertDate = DateTime.Now,
                DaysOffNumber = 22,
                PositionID = 1,
                AccessFailedCount = 0,
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            });

        var roleId = Guid.NewGuid().ToString();
        var roleName = nameof(Enums.Roles.Admin);

        builder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = roleId,
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            });

        builder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId,
            });
    }
}
