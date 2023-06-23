using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.Helpers;

public static class DataSeeder
{
    public static void SeedRootUser(ModelBuilder builder)
    {
        var userId = Guid.NewGuid().ToString();
        var email = "root@root.com";
        var password = "Root123!";
        var positionId = 1;
        var placeholder = "Root";

        var hashedPassword = new PasswordHasher<Employee>().HashPassword(new Employee()
        {
            Id = userId,
            Email = email,
        }, password);

        SeedPosition(builder, positionId, "HR Manager", "Managing HR operations and employee relations.");

        builder.Entity<Employee>()
           .HasData(new
           {
               Id = userId,
               FirstName = placeholder,
               LastName = placeholder,
               UserName = email,
               NormalizedUserName = email.ToUpper(),
               Email = email,
               PasswordHash = hashedPassword,
               IDNumber = "999999",
               Address = placeholder,
               EmployeeStartDate = DateTime.Now,
               InsertDate = DateTime.Now,
               DaysOffNumber = 22,
               OldDaysOffNumber = 0,
               PositionID = positionId,
               AccessFailedCount = 0,
               EmailConfirmed = true,
               LockoutEnabled = false,
               PhoneNumberConfirmed = false,
               TwoFactorEnabled = false,
               SecurityStamp = Guid.NewGuid().ToString("D")
           });

        var roleId = Guid.NewGuid().ToString();
        SeedRole(builder, roleId, nameof(Roles.Admin));
        SeedUserToRole(builder, userId, roleId);
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

    public static void SeedLeaveType(ModelBuilder builder, int id, string caption, string description)
    {
        builder.Entity<LeaveType>()
            .HasData(new LeaveType()
            {
                ID = id,
                Caption = caption,
                Description = description
            });
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
