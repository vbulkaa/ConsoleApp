using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.BLL.Services;
using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Flights;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Statuses;
using FlightManagement.DTO.Stops;

namespace FlightManagement.ASP.Extensions
{
    public static class Endpoints
    {
        private readonly static string _styles = "<style>" +
            "a {display: block; padding: 5px 10px; font-size: 130%; " +
            "background-color: #BBF4FF; text-decoration: none; color: black;" +
            "margin-top: 10px; border-radius: 5px;}" +
            "body {display: flex; flex-direction: column; align-items: center;}" +
            "h1 {text-align: center;}" +
            ".bg-yellow {background-color: #FFF86E;}" +
            ".bg-green {background-color: #49F883;}" +
            "</style>";

        public static string GetHtmlTemplate(string title, string body)
        {
            return "<html><head>" +
                $"<title>{title}</title>" +
                "<meta charset=\"utf-8\" />" +
                _styles +
                "</head><body>" +
                $"<h1>{title}</h1>{body}" +
                "</body></html>";
        }

       
        public static void Info(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(GetHtmlTemplate("Info",
                    "<script type=\"text/javascript\">" +
                    "var code = navigator.appCodeName;" +
                    "var name = navigator.appName;" +
                    "var vers = navigator.appVersion;" +
                    "var platform = navigator.platform;" +
                    "var cook = navigator.cookieEnabled;" +
                    "var je = navigator.javaEnabled();" +
                    "var ua = navigator.userAgent;" +
                    "document.write('Браузер: ' + name + " +
                    "'<br />Версия браузера: ' + vers +" +
                    "'<br />Кодовое название браузера: ' + code +" +
                    "'<br />Платформа: ' + platform +" +
                    "'<br />Поддержка cookie: ' + cook +" +
                    "'<br />Поддержка JavaScript: ' + je +" +
                    "'<br />userAgent: ' + ua);" +
                    "</script>" +
                    "<a href=\"/\">Back</a><br>"));
            });
        }
        public static void AirportsTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IAirportService airportService = context.RequestServices.GetService<IAirportService>();
                IEnumerable<AirportsDto> airports = await airportService.GetAllAirports();

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<table border=1>" +
                    "<tr>" +
                    "<th>Id</th>" +
                    "<th>Name</th>" +
                    "<th>Location</th>" +
                    "</tr>";

                foreach (var airport in airports)
                {
                    htmlString += "<tr>" +
                                $"<td>{airport.AirportID}</td>" +
                                $"<td>{airport.Name}</td>" +
                                $"<td>{airport.Location}</td>" +
                                "</tr>";
                }

                htmlString += "</table>";
                await context.Response.WriteAsync(GetHtmlTemplate("Airports", htmlString));
            });
        }

        public static void FlightsTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IFlightService flightService = context.RequestServices.GetService<IFlightService>();
                IEnumerable<FlightsDto> flights = await flightService.Get(25, "FlightsTable25");

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<table border=1>" +
                    "<tr>" +
                    "<th>Id</th>" +
                    "<th>Flight Number</th>" +
                    "<th>Aircraft Type</th>" +
                    "<th>Ticket Price</th>" +
                    "</tr>";

                foreach (var flight in flights)
                {
                    htmlString += "<tr>" +
                                $"<td>{flight.FlightID}</td>" +
                                $"<td>{flight.FlightNumber}</td>" +
                                $"<td>{flight.AircraftType}</td>" +
                                $"<td>{flight.TicketPrice}</td>" +
                                "</tr>";
                }

                htmlString += "</table>";
                await context.Response.WriteAsync(GetHtmlTemplate("Flights", htmlString));
            });
        }

        public static void RoutesTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IRouteService routeService = context.RequestServices.GetService<IRouteService>();
                IEnumerable<RoutesDto> routes = await routeService.GetAllRoutes();

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<table border=1>" +
                    "<tr>" +
                    "<th>Id</th>" +
                    "<th>Flight ID</th>" +
                    "<th>Departure Time</th>" +
                    "<th>Date</th>" +
                    "</tr>";

                foreach (var route in routes)
                {
                    htmlString += "<tr>" +
                                $"<td>{route.RouteID}</td>" +
                                $"<td>{route.FlightID}</td>" +
                                $"<td>{route.DepartureTime}</td>" +
                                $"<td>{route.Date}</td>" +
                                "</tr>";
                }

                htmlString += "</table>";
                await context.Response.WriteAsync(GetHtmlTemplate("Routes", htmlString));
            });
        }

        public static void StatusesTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IStatusService statusService = context.RequestServices.GetService<IStatusService>();
                IEnumerable<StatusesDto> statuses = await statusService.GetAllStatuses();

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<table border=1>" +
                    "<tr>" +
                    "<th>Id</th>" +
                    "<th>Status Name</th>" +
                    "</tr>";

                foreach (var status in statuses)
                {
                    htmlString += "<tr>" +
                                $"<td>{status.StatusID}</td>" +
                                $"<td>{status.StatusName}</td>" +
                                "</tr>";
                }

                htmlString += "</table>";
                await context.Response.WriteAsync(GetHtmlTemplate("Statuses", htmlString));
            });
        }

        public static void StopsTable(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IStopService stopService = context.RequestServices.GetService<IStopService>();
                IEnumerable<StopsDto> stops = await stopService.GetAllStops();

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<table border=1>" +
                    "<tr>" +
                    "<th>Id</th>" +
                    "<th>Route ID</th>" +
                    "<th>Airport ID</th>" +
                    "<th>Arrival Time</th>" +
                    "<th>Departure Time</th>" +
                    "<th>Status ID</th>" +
                    "</tr>";

                foreach (var stop in stops)
                {
                    htmlString += "<tr>" +
                                $"<td>{stop.StopID}</td>" +
                                $"<td>{stop.RouteID}</td>" +
                                $"<td>{stop.AirportID}</td>" +
                                $"<td>{stop.ArrivalTime}</td>" +
                                $"<td>{stop.DepartureTime}</td>" +
                                $"<td>{stop.StatusID}</td>" +
                                "</tr>";
                }

                htmlString += "</table>";
                await context.Response.WriteAsync(GetHtmlTemplate("Stops", htmlString));
            });


        }

        //Запоминание кукки 
        public static void AirportsSearch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                // Предположим, у вас есть сервис для получения данных об аэропортах
                IAirportService airportsService = context.RequestServices.GetService<IAirportService>();
                IEnumerable<AirportsDto> airports = await airportsService.GetAllAirports();

                string keyName = "AirportName";
                string keySelect = "AirportSelect";
                string keyMultiselect = "AirportMultiselect";

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<form method=\"GET\">" +
                    $"<input id=\"name-input\" type=\"text\" name=\"{keyName}\" placeholder=\"Airport Name\" " +
                    $"value=\"{context.Request.Cookies[keyName]}\"/><br><br>" +
                    $"<select name=\"{keySelect}\"><option>Default</option>";

                foreach (var airport in airports)
                {
                    if ($"{airport.Name} ({airport.AirportID})" == context.Request.Cookies[keySelect])
                        htmlString += $"<option selected>{airport.Name} ({airport.AirportID})</option>";
                    else
                        htmlString += $"<option>{airport.Name} ({airport.AirportID})</option>";
                }

                htmlString += $"</select><br><br><select multiple name=\"{keyMultiselect}\" size=\"10\">";

                string[] selectedItemsArr = { };
                if (context.Request.Cookies[keyMultiselect] is not null)
                    selectedItemsArr = context.Request.Cookies[keyMultiselect].Split(',');

                foreach (var airport in airports)
                {
                    bool isContains = selectedItemsArr.Contains($"{airport.Name} ({airport.AirportID})");

                    if (isContains)
                        htmlString += $"<option selected>{airport.Name} ({airport.AirportID})</option>";
                    else
                        htmlString += $"<option>{airport.Name} ({airport.AirportID})</option>";
                }

                htmlString += "</select><br><br>" +
                    "<button type=\"submit\">Search</button>" +
                    "</form>";

                // Получаем данные из запроса
                string airportName = context.Request.Query[keyName];
                string airportSelected = context.Request.Query[keySelect];
                string airportMultiselected = context.Request.Query[keyMultiselect];

                // Сохраняем данные в куки
                if (airportName is not null)
                    context.Response.Cookies.Append(keyName, airportName);

                if (airportSelected is not null)
                    context.Response.Cookies.Append(keySelect, airportSelected);

                if (airportMultiselected is not null)
                    context.Response.Cookies.Append(keyMultiselect, airportMultiselected);

                await context.Response.WriteAsync(GetHtmlTemplate("Airports", htmlString));
            });
        }

        // запоминание в сессию
        public static void FlightsSearch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                // Предположим, у вас есть сервис для получения данных о рейсах
                IFlightService flightsService = context.RequestServices.GetService<IFlightService>();
                IEnumerable<FlightsDto> flights = await flightsService.GetAllFlights();

                string keyName = "FlightName";
                string keySelect = "FlightSelect";
                string keyMultiselect = "FlightMultiselect";

                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<form>" +
                    $"<input id=\"name-input\" type=\"text\" name=\"{keyName}\" placeholder=\"Flight Name\" " +
                    $"value=\"{context.Session.GetString(keyName)}\"/><br><br>" +
                    $"<select name=\"{keySelect}\"><option>Default</option>";

                foreach (var flight in flights)
                {
                    if ($"{flight.FlightNumber} ({flight.FlightID})" == context.Session.GetString(keySelect))
                        htmlString += $"<option selected>{flight.FlightNumber} ({flight.FlightID})</option>";
                    else
                        htmlString += $"<option>{flight.FlightNumber} ({flight.FlightID})</option>";
                }

                htmlString += $"</select><br><br><select multiple name=\"{keyMultiselect}\" size=\"10\">";

                string[] selectedItemsArr = { };
                if (context.Session.GetString(keyMultiselect) is not null)
                    selectedItemsArr = context.Session.GetString(keyMultiselect).Split(',');

                foreach (var flight in flights)
                {
                    bool isContains = selectedItemsArr.Contains($"{flight.FlightNumber} ({flight.FlightID})");

                    if (isContains)
                        htmlString += $"<option selected>{flight.FlightNumber} ({flight.FlightID})</option>";
                    else
                        htmlString += $"<option>{flight.FlightNumber} ({flight.FlightID})</option>";
                }

                htmlString += "</select><br><br>" +
                    "<button type=\"submit\">Search</button>" +
                    "</form>";

                // Получаем данные из запроса
                string flightName = context.Request.Query[keyName];
                string flightSelected = context.Request.Query[keySelect];
                string flightMultiselected = context.Request.Query[keyMultiselect];

                // Сохраняем данные в сессию
                if (flightName is not null)
                    context.Session.SetString(keyName, flightName);

                if (flightSelected is not null)
                    context.Session.SetString(keySelect, flightSelected);

                if (flightMultiselected is not null)
                    context.Session.SetString(keyMultiselect, flightMultiselected);

                await context.Response.WriteAsync(GetHtmlTemplate("Flights", htmlString));
            });
        }
    }
}
