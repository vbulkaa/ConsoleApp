using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using FlightManagement.DAL.models.enums;

using System;
using System.Collections.Generic;
using FlightManagement.DAL.models.enums;

namespace FlightManagement.DAL
{
    public static class DbInitializer
    {
        public static List<Airports> Airports { get; private set; }
        public static List<Flights> Flights { get; private set; }
        public static List<Statuses> Statuses { get; private set; }
        public static List<Routes> Routes { get; private set; }
        public static List<Stops> Stops { get; private set; }

        private static string[] aircraftTypes = new[]
        {
            "AirbusA320",
            "Boeing737",
            "Embraer175",
            "BombardierCRJ900",
            "Boeing787",
            "AirbusA330",
            "Boeing777",
            "Boeing747",
            "AirbusA350",
            "Embraer190",
            "BombardierDash8Q400",
            "AirbusA380",
            "Boeing767"
        };

        private static Random random = new Random();

        public static void Initialize()
        {
            Airports = new List<Airports>();
            Statuses = new List<Statuses>();
            Flights = new List<Flights>();
            Routes = new List<Routes>();
            Stops = new List<Stops>();

            // Инициализация статусов
            Statuses.Add(new Statuses { StatusID = 1, StatusName = "Вылет" });
            Statuses.Add(new Statuses { StatusID = 2, StatusName = "Промежуточный" });
            Statuses.Add(new Statuses { StatusID = 3, StatusName = "Прилет" });

            // Предоставленный список аэропортов
            var airportsData = new (string Location, string Airport)[]
            {
                ("Abakan", "Abakan International Airport"),
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

            // Инициализация аэропортов
            foreach (var airport in airportsData)
            {
                Airports.Add(new Airports
                {
                    Name = airport.Airport,
                    Location = airport.Location
                });
            }

            // Инициализация рейсов (не менее 500 записей)
            for (int i = 1; i <= 500; i++)
            {
                Flights.Add(new Flights
                {
                    FlightNumber = $"FL-{i:00}",
                    AircraftType = aircraftTypes[random.Next(0, aircraftTypes.Length)], // Используем массив
                    TicketPrice = random.Next(100, 1000)
                });
            }

            // Инициализация маршрутов и остановок (не менее 2000 остановок)
            foreach (var flight in Flights)
            {
                // Создание одного маршрута для каждого рейса
                var route = new Routes
                {
                    FlightID = flight.FlightID, // Убедитесь, что FlightID существует
                    DepartureTime = TimeSpan.FromHours(10 + Flights.IndexOf(flight) % 24),
                    Date = DateTime.Today.AddDays(Flights.IndexOf(flight) % 30)
                };
                Routes.Add(route);

                // Создание остановок
                for (int j = 0; j < 2; j++) // 2 промежуточные остановки
                {
                    var airportIndex = random.Next(0, Airports.Count);
                    var statusIndex = j == 0 ? 1 : 2; // Статус "Вылет" для первой остановки, "Промежуточный" для второй

                    Stops.Add(new Stops
                    {
                        RouteID = route.RouteID,
                        AirportID = Airports[airportIndex].AirportID,
                        ArrivalTime = TimeSpan.FromHours(12 + j),
                        DepartureTime = TimeSpan.FromHours(14 + j),
                        StatusID = statusIndex
                    });
                }

                // Добавляем финальную остановку (прилет)
                var arrivalAirportIndex = random.Next(0, Airports.Count);
                Stops.Add(new Stops
                {
                    RouteID = route.RouteID,
                    AirportID = Airports[arrivalAirportIndex].AirportID,
                    ArrivalTime = TimeSpan.FromHours(16),
                    DepartureTime = TimeSpan.FromHours(17),
                    StatusID = 3 // Прилет
                });
            }
        }
    }
}