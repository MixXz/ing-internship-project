using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Entities;
using VacaYAY.Business.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("VacaYAY")));

builder.Services.AddIdentity<Employee, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<Context>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["APIUrl"]!));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
