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
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Мappings for Airports
            CreateMap<Airports, AirportsDto>();
            CreateMap<AirportsForCreationDto, Airports>();
            CreateMap<AirportsForUpdateDto, Airports>();

            // Мappings for Flights
            CreateMap<Flights, FlightsDto>();
            CreateMap<FlightsForCreationDto, Flights>();
            CreateMap<FlightsForUpdateDto, Flights>();

            // Мappings for Routes
            CreateMap<Routes, RoutesDto>();
            CreateMap<RoutesForCreationDto, Routes>();
            CreateMap<RoutesForUpdateDto, Routes>();

            // Mappings for Statuses
            CreateMap<Statuses, StatusesDto>();
            CreateMap<StatusesForCreationDto, Statuses>();
            CreateMap<StatusesForUpdateDto, Statuses>();

            // Mappings for Stops
            CreateMap<Stops, StopsDto>();
            CreateMap<StopsForCreationDto, Stops>();
            CreateMap<StopsForUpdateDto, Stops>();
        }
    }
}
