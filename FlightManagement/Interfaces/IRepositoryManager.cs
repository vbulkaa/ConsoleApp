using FlightManagement.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces
{
    public interface IRepositoryManager
    {
        IAirportsRepository AirportsRepository { get; }
        IFlightsRepository FlightsRepository { get; }
        IRoutesRepository RoutesRepository { get; }
        IStatusesRepository StatusesRepository { get; }
        IStopsRepository StopsRepository { get; }
    }
}
