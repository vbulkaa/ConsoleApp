namespace FlightManagement.ASPnet.Models
{
    public class FlightSchedule
    {
        public int RouteID { get; set; }
        public string FlightNumber { get; set; }
        public string AircraftType { get; set; }
        public decimal TicketPrice { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportLocation { get; set; }
        public string DepartureStatusName { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalAirportLocation { get; set; }
        public string ArrivalStatusName { get; set; }
    }
}
