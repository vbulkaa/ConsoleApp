using System.Collections.Generic;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.BLL.Services;
using FlightManagement.DAL;
using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Stops;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlightManagement.ASPnet.Extensions
{
    public class Endpoints
    {
        // Получает список аэропортов и отображает его в виде HTML-таблицы
        public static void AirportsTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var logger = context.RequestServices.GetService<ILogger<Endpoints>>();
                var dbContext = context.RequestServices.GetService<AppDbContext>();

                // Проверяем наличие записей во всех таблицах
                bool hasAirports = dbContext.Airports.Any();
                bool hasFlights = dbContext.Flights.Any();
                bool hasStatuses = dbContext.Statuses.Any();
                bool hasRoutes = dbContext.Routes.Any();
                bool hasStops = dbContext.Stops.Any();

                if (!hasAirports || !hasFlights || !hasStatuses || !hasRoutes || !hasStops)
                {
                    // Инициализация данных
                    DbInitializer.Initialize(dbContext, logger);
                    await context.Response.WriteAsync("All Done!");
                }
                else
                {
                    await context.Response.WriteAsync("Tables are already filled!");
                }
            });
        }
    }
}