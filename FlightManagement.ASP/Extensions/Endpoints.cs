using FlightManagement.models;
using FlightManagement.DTO.Airport; // Добавьте этот using для доступа к DTO для аэропортов
using FlightManagement.DTO.Flights; // Для доступа к DTO для рейсов
using FlightManagement.DTO.Stops; // Для доступа к DTO для остановок
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL;
using FlightManagement.DTO.Rotes;

namespace FlightManagement.ASP.Extensions
{
    public static class Endpoints
    {
        //// Получает список аэропортов и отображает его в виде HTML-таблицы
        //public static void AirportsTable(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        IAirportService airportService = context.RequestServices.GetService<IAirportService>();
        //        IEnumerable<AirportsDto> airports = await airportService.GetAll();

        //        IStatusService statusService = context.RequestServices.GetService<IStatusService>();
        //        IFlightService flightService = context.RequestServices.GetService<IFlightService>();
        //        IRouteService routeService = context.RequestServices.GetService<IRouteService>();
        //        IStopService stopService = context.RequestServices.GetService<IStopService>();

        //        // Инициализация данных
        //        if (airports.Count() == 0)
        //        {
        //            // Инициализация данных
        //            DbInitializer.Initialize();

        //            // Получаем все рейсы из базы данных
        //            var flights = await flightService.GetAll();

        //            // Добавление маршрутов с использованием существующих FlightID
        //            foreach (var route in DbInitializer.Routes)
        //            {
        //                // Найдите существующий рейс
        //                var flight = flights.FirstOrDefault(f => f.FlightID == route.FlightID);

        //                if (flight != null) // Убедитесь, что рейс существует
        //                {
        //                    var routeDto = new RoutesForCreationDto
        //                    {
        //                        FlightID = flight.FlightID, // Используйте существующий FlightID
        //                        DepartureTime = route.DepartureTime,
        //                        Date = route.Date
        //                    };
        //                    await routeService.CreateRoute(routeDto);
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"FlightID {route.FlightID} does not exist.");
        //                }
        //            }

        //            // Добавление остановок
        //            foreach (var stop in DbInitializer.Stops)
        //            {
        //                var stopDto = new StopsForCreationDto
        //                {
        //                    RouteID = stop.RouteID,
        //                    AirportID = stop.AirportID,
        //                    ArrivalTime = stop.ArrivalTime,
        //                    DepartureTime = stop.DepartureTime,
        //                    StatusID = stop.StatusID
        //                };
        //                await stopService.CreateStop(stopDto);
        //            }

        //            await context.Response.WriteAsync("All Done!");
        //        }
        //        else
        //        {
        //            await context.Response.WriteAsync("Tables are already filled!");
        //        }


        //    });
        //}
    }
}