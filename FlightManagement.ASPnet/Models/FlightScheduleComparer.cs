namespace FlightManagement.ASPnet.Models
{
    public class FlightScheduleComparer : IEqualityComparer<FlightSchedule>
    {
        public bool Equals(FlightSchedule x, FlightSchedule y)
        {
            return x.RouteID == y.RouteID &&
                   x.FlightNumber == y.FlightNumber &&
                   x.DepartureTime == y.DepartureTime &&
                   x.ArrivalTime == y.ArrivalTime;
        }

        public int GetHashCode(FlightSchedule obj)
        {
            return HashCode.Combine(obj.RouteID, obj.FlightNumber, obj.DepartureTime, obj.ArrivalTime);
        }
    }
}
