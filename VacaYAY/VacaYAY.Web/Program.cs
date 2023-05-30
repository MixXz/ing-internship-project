using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("VacaYAY")));

builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
        .AddEntityFrameworkStores<Context>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.User.RequireUniqueEmail = false)
//    .AddEntityFrameworkStores<Context>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();