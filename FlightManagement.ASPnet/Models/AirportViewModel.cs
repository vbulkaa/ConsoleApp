using FlightManagement.DTO.Airport;
using FlightManagement.models;

namespace FlightManagement.ASPnet.Models
{
    public class AirportViewModel
    {
        public IEnumerable<AirportsDto> Airports { get; set; }
        public string SearchTerm { get; set; }
        public string SortOrder { get; set; }
    }
}
