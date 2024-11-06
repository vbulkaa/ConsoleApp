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
//Регистация контроллеров
builder.Services.AddControllers();

// Регистрация сервисов
builder.Services.ConfigureDbServices();

// Настройка AutoMapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile()); //Содержит правила маппинга
});
IMapper autoMapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(autoMapper); 

// Настройка кеширования и сессий
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(); //поддержку сессий, что позволяет хранить состояние пользователя между запросами

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Использование сессий
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

// Настройка маршрутов
//app.Map("/Table", Endpoints.);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// Запуск приложения
app.Run();
