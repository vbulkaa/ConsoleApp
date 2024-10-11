using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.BLL.Services;
using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Flights;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Statuses;
using FlightManagement.DTO.Stops;

namespace FlightManagement.ASP.Extensions
{
    /*Отвечает за настройку конечных точек(endpoints) для обработки http запросов*/
    public static class Endpoints
    {
        private readonly static string _styles = "<style>" +
            "a {display: block; padding: 10px 10px; font-size: 150%; " +
            "background-color: #BBF4FF; text-decoration: none; color: black;" +
            "margin-top: 15px; border-radius: 0px;}" +
            "body {display: flex; flex-direction: column; align-items: center;}" +
            "h1 {text-align: center;}" +
            ".bg-yellow {background-color: #FFF86E;}" +
            ".bg-green {background-color: #49F883;}" +
            "</style>";

        //Создает html-шаблон для страницы
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

       //Обрабатывает запросы на получение инфо о браузере
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

        //Получает список аэропортов и отображает его в виде HTML-таблицы
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

        //Получает список рейсов и отображает его в виде HTML-таблицы
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

        //Предоставляет форму для поиска рейсов с
        //возможностью запоминания выбранных значений в сессии.
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

        //Запоминание куки 
        //Предоставляет форму для поиска аэропортов с
        //возможностью запоминания выбранных значений в cookies.
        public static void AirportsSearch(IApplicationBuilder app)
        {
            app.Run(async context => // определяет конечную точку, которая будет обрабатывать HTTP-запросы
            {
                
                IAirportService airportsService = context.RequestServices.GetService<IAirportService>(); //Получаем данные об аэропортах
                IEnumerable<AirportsDto> airports = await airportsService.GetAllAirports(); 

                //Ключи куки( для хранения данных), они будут использоваться для доступа к значениям,
                //которые вводит пользователь
                string keyName = "AirportName";
                string keySelect = "AirportSelect";
                string keyMultiselect = "AirportMultiselect";

                //HTML код для страницы 
                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<form method=\"GET\">" +
                    $"<input id=\"name-input\" type=\"text\" name=\"{keyName}\" placeholder=\"Airport Name\" " + //Cоздается поле ввода для имени аэропорта, 
                    $"value=\"{context.Request.Cookies[keyName]}\"/><br><br>" + //если есть значение по ключу 
                    $"<select name=\"{keySelect}\"><option>Default</option>"; //Оно будет отображаться в поле

                //Для каждого аэропорта из списка airports добавляется эл options в выпадающий список
                //Если аэропорт соответствует значению, сохраненному в кукки по ключу keySelect, он будет помечен как выбранный
                foreach (var airport in airports)
                {
                    if ($"{airport.Name} ({airport.AirportID})" == context.Request.Cookies[keySelect])
                        htmlString += $"<option selected>{airport.Name} ({airport.AirportID})</option>";
                    else
                        htmlString += $"<option>{airport.Name} ({airport.AirportID})</option>";
                }

                //Создание многострочного выпадающего списка
                htmlString += $"</select><br><br><select multiple name=\"{keyMultiselect}\" size=\"10\">";
                

                string[] selectedItemsArr = { };
                if (context.Request.Cookies[keyMultiselect] is not null)
                    selectedItemsArr = context.Request.Cookies[keyMultiselect].Split(',');

                //Каждому аэропорту добавляется элемент <option>, и если он содержится в selectedItemsArr,
                //он будет отмечен как выбранный
                foreach (var airport in airports)
                {
                    bool isContains = selectedItemsArr.Contains($"{airport.Name} ({airport.AirportID})");

                    if (isContains)
                        htmlString += $"<option selected>{airport.Name} ({airport.AirportID})</option>";
                    else
                        htmlString += $"<option>{airport.Name} ({airport.AirportID})</option>";
                }

                //Завершение списка и добавление поиска
                htmlString += "</select><br><br>" +
                    "<button type=\"submit\">Search</button>" +
                    "</form>";

                // Получаем данные из запроса
                string airportName = context.Request.Query[keyName];
                string airportSelected = context.Request.Query[keySelect];
                string airportMultiselected = context.Request.Query[keyMultiselect];

                // Сохраняем данные в куки
                if (airportName is not null)
                    context.Response.Cookies.Append(keyName, airportName); //добавление куки в ответ, который будет отправлен клиенту

                if (airportSelected is not null)
                    context.Response.Cookies.Append(keySelect, airportSelected);

                if (airportMultiselected is not null)
                    context.Response.Cookies.Append(keyMultiselect, airportMultiselected);

                //Ответ
                await context.Response.WriteAsync(GetHtmlTemplate("Airports", htmlString));
            });
        }

        // запоминание в сессию

        public static void FlightsSearch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                
                IFlightService flightsService = context.RequestServices.GetService<IFlightService>();
                IEnumerable<FlightsDto> flights = await flightsService.GetAllFlights();

                //Определяются строки, которые будут использоваться как ключи для хранения данных в сессии
                string keyName = "FlightName";
                string keySelect = "FlightSelect";
                string keyMultiselect = "FlightMultiselect";

                //Формирование строки, которая будет содержать html код
                string htmlString =
                    "<a href=\"/\">Back</a><br>" +
                    "<form>" +
                    $"<input id=\"name-input\" type=\"text\" name=\"{keyName}\" placeholder=\"Flight Name\" " +
                    $"value=\"{context.Session.GetString(keyName)}\"/><br><br>" +
                    $"<select name=\"{keySelect}\"><option>Default</option>";

                //Заполнение выпадающего списка
                foreach (var flight in flights)
                {
                    if ($"{flight.FlightNumber} ({flight.FlightID})" == context.Session.GetString(keySelect))
                        htmlString += $"<option selected>{flight.FlightNumber} ({flight.FlightID})</option>";
                    else
                        htmlString += $"<option>{flight.FlightNumber} ({flight.FlightID})</option>";
                }

                //Создание многострочного выпадающего списка
                htmlString += $"</select><br><br><select multiple name=\"{keyMultiselect}\" size=\"10\">";

                //Извлечение выбранных значений
                string[] selectedItemsArr = { };
                if (context.Session.GetString(keyMultiselect) is not null)
                    selectedItemsArr = context.Session.GetString(keyMultiselect).Split(',');

                //Заполнение многострочного выпадающего списка
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
