using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Entities;
using VacaYAY.Business.Services;
using SendGrid.Extensions.DependencyInjection;
using Quartz;
using VacaYAY.Business.Jobs;

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
    var jobKey = new JobKey("NotifyJob");
    q.AddJob<NotifyJob>(opts => opts.WithIdentity(new JobKey("NotifyJob")));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("NotifyJob-trigger")
        .WithCronSchedule("0 0 */1 * * ?")); //run every 1 hour.
        //.WithCronSchedule("0/5 * * * * ?")); //run every 5 second.
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<INotifierSerivice, NotifierService>();

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
