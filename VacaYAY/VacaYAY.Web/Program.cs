using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;
using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Entities;
using VacaYAY.Business.Services;
using SendGrid.Extensions.DependencyInjection;
using Quartz;
using VacaYAY.Business.Jobs;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.RepositoryContracts;
using VacaYAY.Data.Services;
using VacaYAY.Data.DataServiceContracts;
using VacaYAY.Data.DataService;

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

builder.Services.AddSendGrid(options =>
    options.ApiKey = builder.Configuration.GetSection("EmailSenderSettings").GetValue<string>("APIKey"));

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.AddJobAndTrigger<NotifyOfRequestStatusJob>(builder.Configuration);
    q.AddJobAndTrigger<NotifyOfRemainingDaysOffJob>(builder.Configuration);
    q.AddJobAndTrigger<RemoveOldDaysOffJob>(builder.Configuration);
    q.AddJobAndTrigger<AddNewDaysOffJob>(builder.Configuration);
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<INotifierService, NotifierService>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

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

app.UseNotyf();

app.MapRazorPages();

app.Run();
