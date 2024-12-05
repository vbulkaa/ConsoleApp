using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.models
{
    [Table("Routes")]
    public class Route
    {
        public int RouteID { get; set; }
        [ForeignKey("Flight")]
        public int FlightID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }

        public virtual Flight Flight { get; set; }
        public ICollection<Stop> Stops { get; set; }
     
    }
}
