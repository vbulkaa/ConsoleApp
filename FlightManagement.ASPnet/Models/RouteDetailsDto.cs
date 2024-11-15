using FlightManagement.DTO.Stops;

namespace FlightManagement.ASPnet.Models
{
    public class RouteDetailsDto
    {
        public int RouteID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }
        public List<StopsDto> Stops { get; set; }
    }
    public class StopDto
    {
        public int StopID { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string AirportName { get; set; }
        public string AirportLocation { get; set; }
        public string StatusName { get; set; }
    }

 
}
