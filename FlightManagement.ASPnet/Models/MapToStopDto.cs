using FlightManagement.DTO.Stops;

namespace FlightManagement.ASPnet.Models
{
    public static class DtoMapper
    {
        public static StopDto MapToStopDto(StopsDto stopsDto)
        {
            return new StopDto
            {
                StopID = stopsDto.StopID,
                ArrivalTime = stopsDto.ArrivalTime,
                DepartureTime = stopsDto.DepartureTime,
                AirportName = stopsDto.AirportID.ToString(), // Замените на правильное значение
                AirportLocation = "Location", // Замените на правильное значение
                StatusName = stopsDto.StatusID.ToString() // Замените на правильное значение
            };
        }
    }
}
