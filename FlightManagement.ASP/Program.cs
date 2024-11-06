using FlightManagement.DAL.Configuration;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlightManagement.DAL;
using FlightManagement.ASP.Extensions;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllersWithViews();
//���������� ������������
builder.Services.AddControllers();

// ����������� ��������
builder.Services.ConfigureDbServices();

// ��������� AutoMapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile()); //�������� ������� ��������
});
IMapper autoMapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(autoMapper); 

// ��������� ����������� � ������
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(); //��������� ������, ��� ��������� ������� ��������� ������������ ����� ���������

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ������������� ������
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

// ��������� ���������
//app.Map("/Table", Endpoints.);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// ������ ����������
app.Run();
