using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Stops
{
    public class StopsDto
    {
        public int StopID { get; set; }
        public int RouteID { get; set; }
        public int AirportID { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int StatusID { get; set; }

        // Добавляем новые свойства
        public string StatusName { get; set; }
        public string AirportName { get; set; }
        public string AirportLocation { get; set; }
    }
}
