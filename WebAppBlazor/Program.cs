using CurrieTechnologies.Razor.SweetAlert2;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using WebAppBlazor.Data;
using WebAppBlazor.Pages.Application;
using WebAppBlazor.Services;
using WebAppBlazor.Services.Attendance;
using WebAppBlazor.Services.Department;
using WebAppBlazor.Services.Employee;
using WebAppBlazor.Services.WorkHoursSummary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add scoped for service
builder.Services.AddScoped<WeatherForecastService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IWorkHoursSummaryService, WorkHoursSummaryService>();
// Sweet alert
builder.Services.AddSweetAlert2();
// Material UI
builder.Services.AddMudServices();
// Config GRPC Client Factory
builder.Services.AddGRPCClient();
builder.Services.AddAuthenticationService();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.UseMiddleware<ExceptionMiddleware>();
app.MapFallbackToPage("/_Host");

app.Run();
