using Microsoft.EntityFrameworkCore;
using Core_Prac_01.Models;

//ConfigureService
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicantDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddControllersWithViews();
var app = builder.Build();

//Configure
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
