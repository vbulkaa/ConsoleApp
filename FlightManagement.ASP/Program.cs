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

var app = builder.Build();

// Использование сессий
app.UseSession();

// Настройка маршрутов
app.Map("/Airports", Endpoints.AirportsTable);
app.Map("/Flights", Endpoints.FlightsTable);
app.Map("/Routes", Endpoints.RoutesTable);
app.Map("/Statuses", Endpoints.StatusesTable);
app.Map("/Stops", Endpoints.StopsTable);
app.Map("/Info", Endpoints.Info);
app.Map("/searchform1", Endpoints.AirportsSearch);
app.Map("/searchform2", Endpoints.FlightsSearch);


app.MapGet("/", async context =>
{
    string htmlString = Endpoints.GetHtmlTemplate("Index",
        "<a href=\"/Airports\">Airports</a>" +
        "<a href=\"/Flights\">Flights</a>" +
        "<a href=\"/Routes\">Routes</a>" +
        "<a href=\"/Statuses\">Statuses</a>" +
        "<a href=\"/Stops\">Stops</a>" +
        "<a href=\"/Info\" class=\"bg-yellow\">Info</a>" +
        "<a href=\"/searchform1\">Search Airports</a>" +
        "<a href=\"/searchform2\">Search Flights</a>"); 

    await context.Response.WriteAsync(htmlString); 
});


// Запуск приложения
app.Run();
