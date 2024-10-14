using FlightManagement.DAL.Configuration;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlightManagement.DAL;
using FlightManagement.ASPnet.Extensions;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервиса с использованием Scoped lifetime
//builder.Services.AddScoped<IMyService, MyService>();

//Регистация контроллеров
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// Использование сессий
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

// Настройка маршрутов
app.Map("/Airports", Endpoints.AirportsTable);


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


