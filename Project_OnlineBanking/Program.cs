using Microsoft.EntityFrameworkCore;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserService, UserServiceImpl>();
builder.Services.AddScoped<TransactionService, TransactionServiceImpl>();
builder.Services.AddScoped<MailService, MailServiceImpl>();

builder.Services.AddSession();

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller}/{action}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}"
);

app.Run();
