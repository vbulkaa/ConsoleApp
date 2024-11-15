using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.models
{
    [Table("Stops")]
    public class Stops
    {
        public int StopID { get; set; }
        public int RouteID { get; set; }
        public int AirportID { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int StatusID { get; set; }

        public Routes Route { get; set; }
        public Airports Airport { get; set; }
        public Statuses Status { get; set; }
        
    }
}
