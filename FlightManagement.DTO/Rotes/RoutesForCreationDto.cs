using FlightManagement.DTO.Stops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Rotes
{
    public class RoutesForCreationDto
    {
        public int FlightID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }
        public List<StopsForCreationDto> Stops { get; set; }
    }
}
