using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.BLL.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.ASPnet.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
                    b.MigrationsAssembly("FlightManagement.DAL")));
        }

        public static void ConfigureDbServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            // Добавление сервисов для управления рейсами, аэропортами и т.д.
            services.AddScoped<IAirportService, AirportService>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IStopService, StopService>();
        }
    }
}
