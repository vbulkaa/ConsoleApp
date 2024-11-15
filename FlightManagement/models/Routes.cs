using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.models
{
    [Table("Routes")]
    public class Routes
    {
        public int RouteID { get; set; }
        public int FlightID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }

        public virtual Flights Flight { get; set; }
        public ICollection<Stops> Stops { get; set; }
       // public virtual Airports Airport { get; set; }
      //  public object AirportID { get; set; }
    }
}
