using FlightManagement.models;
using System;
using AutoMapper;
using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Flights;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Statuses;
using FlightManagement.DTO.Stops;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL
{
    //для преобразования объектов между различными слоями приложения
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Правила маппинга
            CreateMap<Airport, AirportsDto>();
            CreateMap<AirportsForCreationDto, Airport>();
            CreateMap<AirportsForUpdateDto, Airport>();

            
            CreateMap<Flight, FlightsDto>();
            CreateMap<FlightsForCreationDto, Flight>();
            CreateMap<FlightsForUpdateDto, Flight>();

           
            CreateMap<Route, RoutesDto>();
            CreateMap<RoutesForCreationDto, Route>();
            CreateMap<RoutesForUpdateDto, Route>();

            
            CreateMap<Status, StatusesDto>();
            CreateMap<StatusesForCreationDto, Status>();
            CreateMap<StatusesForUpdateDto, Status>();

            
            CreateMap<Stop, StopsDto>();
            CreateMap<StopsForCreationDto, Stop>();
            CreateMap<StopsForUpdateDto, Stop>();
        }

    }
}
