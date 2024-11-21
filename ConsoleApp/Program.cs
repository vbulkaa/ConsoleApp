// See https://aka.ms/new-console-template for more information
using FlightManagement;
using FlightManagement.DAL;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;


var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

var builder = new DbContextOptionsBuilder<AppDbContext>();
builder.UseSqlServer(connectionString);

using (var context = new AppDbContext(builder.Options))
{
    var query = new Query(connectionString);

    bool exit = false;

    while (!exit)
    {
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Выборка всех аэропортов");
        Console.WriteLine("2. Фильтрация аэропортов по имени");
        Console.WriteLine("3. Получить количество маршрутов для рейса");
        Console.WriteLine("4. Получить рейсы и их маршруты");
        Console.WriteLine("5. Фильтрация остановок по рейсу");
        Console.WriteLine("6. Добавить аэропорт");
        Console.WriteLine("7. Добавить остановку");
        Console.WriteLine("8. Удалить аэропорт");
        Console.WriteLine("9. Удалить остановку");
        Console.WriteLine("10. Обновить цену билета рейса");
        Console.WriteLine("0. Выход");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var airports = query.GetAllAirports();
                foreach (var airport in airports)
                {
                    Console.WriteLine($"ID: {airport.AirportID}, Name: {airport.Name}, Location: {airport.Location}");
                }
                break;

            case "2":
                Console.Write("Введите имя аэропорта для фильтрации: ");
                var name = Console.ReadLine();
                var filteredAirports = query.GetFilteredAirports(name);
                foreach (var airport in filteredAirports)
                {
                    Console.WriteLine($"ID: {airport.AirportID}, Name: {airport.Name}, Location: {airport.Location}");
                }
                break;

            case "3":
                Console.Write("Введите ID рейса: ");
                int flightId = int.Parse(Console.ReadLine());
                int routeCount = query.GetRouteCountByFlight(flightId);
                Console.WriteLine($"Количество маршрутов для рейса {flightId}: {routeCount}");
                break;

            case "4":
                var flightRoutes = query.GetFlightRoutes();
                foreach (var (flightNumber, routeDate) in flightRoutes)
                {
                    Console.WriteLine($"Рейс: {flightNumber}, Дата маршрута: {routeDate}");
                }
                break;

            case "5":
                Console.Write("Введите ID рейса: ");
                flightId = int.Parse(Console.ReadLine());
                var filteredStops = query.GetFilteredStopsByFlight(flightId);
                foreach (var stop in filteredStops)
                {
                    Console.WriteLine($"Стоп ID: {stop.StopID}, Аэропорт ID: {stop.AirportID}");
                }
                break;

            case "6":
                var newAirport = new Airport();
                Console.Write("Введите имя аэропорта: ");
                newAirport.Name = Console.ReadLine();
                Console.Write("Введите местоположение аэропорта: ");
                newAirport.Location = Console.ReadLine();
                query.AddAirport(newAirport);
                break;

            case "7":
                var newStop = new Stop();
                Console.Write("Введите ID маршрута: ");
                newStop.RouteID = int.Parse(Console.ReadLine());
                Console.Write("Введите ID аэропорта: ");
                newStop.AirportID = int.Parse(Console.ReadLine());
                Console.Write("Введите время прибытия (hh:mm:ss): ");
                newStop.ArrivalTime = TimeSpan.Parse(Console.ReadLine());
                Console.Write("Введите время отправления (hh:mm:ss): ");
                newStop.DepartureTime = TimeSpan.Parse(Console.ReadLine());
                Console.Write("Введите ID статуса: ");
                newStop.StatusID = int.Parse(Console.ReadLine());
                query.AddStop(newStop);
                break;

            case "8":
                Console.Write("Введите ID аэропорта для удаления: ");
                int airportId = int.Parse(Console.ReadLine());
                query.DeleteAirport(airportId);
                break;

            case "9":
                Console.Write("Введите ID остановки для удаления: ");
                int stopId = int.Parse(Console.ReadLine());
                query.DeleteStop(stopId);
                break;

            case "10":
                Console.Write("Введите ID рейса: ");
                flightId = int.Parse(Console.ReadLine());
                Console.Write("Введите новую цену билета: ");
                decimal newPrice = decimal.Parse(Console.ReadLine());
                query.UpdateFlightTicketPrice(flightId, newPrice);
                break;

            case "0":
                exit = true;
                break;

            default:
                Console.WriteLine("Некорректный выбор.");
                break;
        }
    }
}
