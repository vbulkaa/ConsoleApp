using FlightManagement.models;
using Microsoft.Extensions.Logging;
using static System.Formats.Asn1.AsnWriter;


namespace FlightManagement.DAL
{
    public class DbInitializer
    {
        //private readonly AppDbContext _context;
        //private readonly ILogger<DbInitializer> _logger;
        private static readonly Random _random = new Random();

        public static List<Airports> Airports { get; private set; }
        public static List<Flights> Flights { get; private set; }

        public static List<Routes> Routes { get; private set; } 
        public static List<Stops> Stops { get; private set; } 
        public static List<Statuses> Statuss { get; private set; }

        
        public static void Initialize(AppDbContext context, ILogger logger)
        {
            // Убедиться, что база данных создана
            context.Database.EnsureCreated();

            /*if (context.Airports.Any() || context.Flights.Any() || context.Statuses.Any() || context.Routes.Any() || context.Stops.Any())
            {
                logger.LogInformation("Database already initialized.");
                return; 
            }*/

            var airportsData = new (string Location, string Airport)[]
            { ("Abakan", "Abakan International Airport"),
                ("Anadyr", "Ugolny Airport"),
                ("Anapa", "Anapa Airport"),
                ("Arkhangelsk", "Talagi Airport"),
                ("Astrakhan", "Narimanovo Airport"),
                ("Barnaul", "Barnaul Airport"),
                ("Belgorod", "Belgorod International Airport"),
                ("Blagoveshchensk", "Ignatyevo Airport"),
                ("Bratsk", "Bratsk Airport"),
                ("Bryansk", "Bryansk International Airport"),
                ("Cheboksary", "Cheboksary International Airport"),
                ("Chelyabinsk", "Chelyabinsk Airport"),
                ("Cherepovets", "Cherepovets Airport"),
                ("Chita", "Chita-Kadala International Airport"),
                ("Elista", "Elista Airport"),
                ("Irkutsk", "Irkutsk Airport"),
                ("Grozny", "Grozny Airport"),
                ("Kaliningrad", "Khrabrovo Airport"),
                ("Kazan", "Kazan Airport"),
                ("Khabarovsk", "Khabarovsk Novy Airport"),
                ("Komsomolsk-on-Amur", "Komsomolsk-on-Amur Airport"),
                ("Krasnodar", "Pashkovsky Airport"),
                ("Krasnoyarsk", "Yemelyanovo International Airport"),
                ("Kursk", "Kursk Vostochny Airport"),
                ("Magadan", "Sokol Airport"),
                ("Magnitogorsk", "Magnitogorsk International Airport"),
                ("Makhachkala", "Uytash Airport"),
                ("Mineralnye Vody", "Mineralnye Vody Airport"),
                ("Moscow", "Domodedovo International Airport"),
                ("Zhukovsky International Airport", "ZIA"),
                ("Sheremetyevo International Airport", "SVO"),
                ("Vnukovo Airport", "VKO"),
                ("Murmansk", "Murmansk Airport"),
                ("Nalchik", "Nalchik Airport"),
                ("Nizhnevartovsk", "Nizhnevartovsk Airport"),
                ("Nizhnekamsk", "Begishevo Airport"),
                ("Nizhny Novgorod", "Strigino Airport"),
                ("Novokuznetsk", "Spichenkovo Airport"),
                ("Novosibirsk", "Tolmachevo Airport"),
                ("Omsk", "Omsk Tsentralny Airport"),
                ("Orenburg", "Orenburg Tsentralny Airport"),
                ("Orsk", "Orsk Airport"),
                ("Perm", "Perm International Airport"),
                ("Petrozavodsk", "Petrozavodsk Airport"),
                ("Provideniya", "Provideniya Bay Airport"),
                ("Petropavlovsk-Kamchatsky", "Yelizovo Airport"),
                ("Pskov", "Pskov Airport"),
                ("Rostov-on-Don", "Platov International Airport"),
                ("Saint Petersburg", "Pulkovo Airport"),
                ("Samara", "Samara Kurumoch Airport"),
                ("Saratov", "Saratov Gagarin Airport"),
                ("Sochi", "Sochi International Airport"),
                ("Stavropol", "Stavropol Shpakovskoye Airport"),
                ("Surgut", "Surgut International Airport"),
                ("Syktyvkar", "Syktyvkar Airport"),
                ("Tomsk", "Bogashevo Airport"),
                ("Tyumen", "Roshchino International Airport"),
                ("Ulan-Ude", "Baikal International Airport"),
                ("Ulyanovsk", "Ulyanovsk Baratayevka Airport"),
                ("Ufa", "Ufa International Airport"),
                ("Vladivostok", "Vladivostok International Airport"),
                ("Vladikavkaz", "Beslan Airport"),
                ("Volgograd", "Volgograd International Airport"),
                ("Voronezh", "Voronezh International Airport"),
                ("Yakutsk", "Yakutsk Airport"),
                ("Yaroslavl", "Tunoshna Airport"),
                ("Yekaterinburg", "Koltsovo International Airport"),
                ("Yuzhno-Sakhalinsk", "Yuzhno-Sakhalinsk Airport"),
                ("Grodno", "Hrodna Airport"),
                ("Gomel", "Gomel Airport"),
                ("Minsk", "Minsk International Airport")

            };

            // Инициализация статусов
            var statuses = new List<Statuses>
            {
                 new Statuses { StatusName = "Вылет" },
                 new Statuses { StatusName = "Промежуточный" },
                 new Statuses { StatusName = "Прилет" }
            };
            
            context.Statuses.AddRange(statuses);
            context.SaveChanges();

            // Инициализация аэропортов
            var airports = airportsData.Select(airport => new Airports
            {
                Name = airport.Airport,
                Location = airport.Location
            }).ToList();

            context.Airports.AddRange(airports);
            context.SaveChanges();
            logger.LogInformation($"{airports.Count} airports initialized.");
            // Инициализация рейсов
            var flights = new List<Flights>();
            var aircraftTypes = new[]
            {
                "AirbusA320", "Boeing737", "Embraer175", "BombardierCRJ900",
                "Boeing787", "AirbusA330", "Boeing777", "Boeing747",
                "AirbusA350", "Embraer190", "BombardierDash8Q400", "AirbusA380", "Boeing767"
            };

            var random = new Random();
            for (int i = 1; i <= 2000; i++)
            {
                var flight = new Flights
                {
                    FlightNumber = $"FL-{i:00}",
                    AircraftType = aircraftTypes[random.Next(aircraftTypes.Length)],
                    TicketPrice = random.Next(100, 1000)
                };
                flights.Add(flight);
            }

            context.Flights.AddRange(flights);
            context.SaveChanges();
            logger.LogInformation($"{flights.Count} flights initialized.");
            
            var routes = new List<Routes>(); 
            var stops = new List<Stops>();

            // Инициализация маршрутов и остановок
            foreach (var flight in flights)
            {
                var route = new Routes
                {
                    RouteID = flight.FlightID, // Используем FlightID как RouteID
                    FlightID = flight.FlightID,
                    DepartureTime = TimeSpan.FromHours(random.Next(5, 22)), // Случайное время вылета
                    Date = DateTime.Today.AddDays(random.Next(0, 30)) // Случайная дата в пределах 30 дней
                };

                context.Routes.Add(route);
                context.SaveChanges(); // Сохраняем маршрут, чтобы получить его ID
                logger.LogInformation($"Route for flight {flight.FlightNumber} initialized.");

                // Создаем 3 остановки
                for (int j = 1; j <= 3; j++)
                {
                    var airportIndex = random.Next(airports.Count);
                    var statusIndex = j == 61 ? 61 : (j == 62 ? 62 : 63); // Вылет, промежуточный, прилет

                    var stop = new Stops
                    {
                        StopID = flight.FlightID * 10 + j, // Уникальный StopID
                        RouteID = route.RouteID,
                        AirportID = airports[airportIndex].AirportID,
                        ArrivalTime = route.DepartureTime.Add(TimeSpan.FromHours(j)), // Время прибытия через 1, 2, 3 часа
                        DepartureTime = route.DepartureTime.Add(TimeSpan.FromHours(j + 0.5)), // Отправление через 30 минут после прибытия
                        StatusID = statusIndex // Устанавливаем статус
                    };

                    context.Stops.Add(stop);
                }
            }

            context.SaveChanges(); // Сохраняем все остановки в конце
            logger.LogInformation($"{context.Routes.Count()} routes and {context.Stops.Count()} stops initialized.");
        }

    }
}