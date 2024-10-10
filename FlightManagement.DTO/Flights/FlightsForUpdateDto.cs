using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Flights
{
    public class FlightsForUpdateDto
    {
        public string FlightNumber { get; set; }
        public string AircraftType { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
