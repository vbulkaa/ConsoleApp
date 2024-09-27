using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.models
{
    [Table("Airports")]
    public class Airports
    {
        public int AirportID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}
