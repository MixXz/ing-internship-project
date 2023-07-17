using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VacaYAY.Data.DataTransferObjects.Employees;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.Helpers;

internal static class DataSeeder
{
    public static void SeedRootUser(ModelBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("AppSettings:DefaultData:Employee");

        if (section is null)
        {
            throw new ArgumentException("Sections not found.");
        }

        var employee = section.Get<EmployeeCreate>()!;
        var employeeId = Guid.NewGuid().ToString();

        var position = section.GetSection(nameof(Position)).Get<Position>()!;
        position.ID = 1;

        var hashedPassword = new PasswordHasher<Employee>().HashPassword(new Employee()
        {
            Id = employeeId,
            Email = employee.Email,
        }, employee.Password);

        SeedPosition(builder, 1, position.Caption, position.Description);

        builder.Entity<Employee>()
           .HasData(new
           {
               Id = employeeId,
               FirstName = employee.FirstName,
               LastName = employee.LastName,
               UserName = employee.Email,
               NormalizedUserName = employee.Email.ToUpper(),
               Email = employee.Email,
               PasswordHash = hashedPassword,
               IDNumber = employee.IDNumber,
               Address = employee.FirstName,
               EmployeeStartDate = DateTime.Now,
               InsertDate = DateTime.Now,
               DaysOffNumber = employee.DaysOffNumber,
               OldDaysOffNumber = 0,
               PositionID = position.ID,
               AccessFailedCount = 0,
               EmailConfirmed = true,
               LockoutEnabled = false,
               PhoneNumberConfirmed = false,
               TwoFactorEnabled = false,
               SecurityStamp = Guid.NewGuid().ToString("D")
           });

        var roleId = Guid.NewGuid().ToString();
        SeedRole(builder, roleId, nameof(Roles.Admin));
        SeedUserToRole(builder, employeeId, roleId);
    }

    public static void SeedPosition(ModelBuilder builder, int id, string caption, string description)
    {
        builder.Entity<Position>()
            .HasData(new Position()
            {
                ID = id,
                Caption = caption,
                Description = description
            });
    }

    public static void SeedPositions(ModelBuilder builder, IConfiguration config)
    {
        var positions = config.GetSection("AppSettings:DefaultData:Positions").Get<List<Position>>();

        if (positions is null)
        {
            throw new ArgumentException("Sections not found.");
        }

        for (int i = 0; i < positions.Count(); i++)
        {
            SeedPosition(builder, i + 2, positions[i].Caption, positions[i].Description);
        }
    }

    public static void SeedLeaveTypes(ModelBuilder builder, IConfiguration config)
    {
        var leaveTypes = config.GetSection("AppSettings:DefaultData:LeaveTypes").Get<List<LeaveType>>();

        if (leaveTypes is null)
        {
            throw new ArgumentException("Sections not found.");
        }

        leaveTypes.Add(new()
        {
            Caption = VacationType.CollectiveVacation,
            Description = VacationType.CollectiveVacation
        });

        for (int i = 0; i < leaveTypes.Count(); i++)
        {
            builder.Entity<LeaveType>()
                .HasData(new LeaveType()
                {
                    ID = i + 1,
                    Caption = leaveTypes[i].Caption,
                    Description = leaveTypes[i].Description
                });
        }
    }

    public static void SeedRole(ModelBuilder builder, string id, string roleName)
    {
        builder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = id,
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            });
    }

    public static void SeedUserToRole(ModelBuilder builder, string userId, string roleId)
    {
        builder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId,
            });
    }
}
