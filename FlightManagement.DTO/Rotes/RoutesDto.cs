using FlightManagement.DTO.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Rotes
{
    public class RoutesDto
    {
        public int RouteID { get; set; }
        public int FlightID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }

        public FlightsDto Flight { get; set; }
    }
}
