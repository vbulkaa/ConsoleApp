using FlightManagement.DAL.Configuration;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlightManagement.DAL;
using FlightManagement.ASPnet.Extensions;
using AutoMapper;
using Microsoft.OpenApi.Models;
using FlightManagement.DAL.models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ���������� �����������
builder.Logging.ClearProviders();  // ������� ���������� ����������� �� ���������
builder.Logging.AddConsole();

// ����������� ������� � �������������� Scoped lifetime
//builder.Services.AddScoped<IMyService, MyService>();

//���������� ������������
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitializeAsync(roleManager);
}

/*builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
// ������������� ������
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

// Swagger configuration in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Access Swagger UI at the root URL
});

// ��������� ���������
app.Map("/Inicialize", Endpoints.Table);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


