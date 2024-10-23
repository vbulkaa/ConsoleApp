using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Stops
{
    public class StopsForUpdateDto
    {
        public int StopID { get; set; }

        public int RouteID { get; set; }
        public int AirportID { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int StatusID { get; set; }
    }
}
