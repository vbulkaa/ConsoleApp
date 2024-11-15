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
            CreateMap<Airports, AirportsDto>();
            CreateMap<AirportsForCreationDto, Airports>();
            CreateMap<AirportsForUpdateDto, Airports>();

            
            CreateMap<Flights, FlightsDto>();
            CreateMap<FlightsForCreationDto, Flights>();
            CreateMap<FlightsForUpdateDto, Flights>();

           
            CreateMap<Routes, RoutesDto>();
            CreateMap<RoutesForCreationDto, Routes>();
            CreateMap<RoutesForUpdateDto, Routes>();

            
            CreateMap<Statuses, StatusesDto>();
            CreateMap<StatusesForCreationDto, Statuses>();
            CreateMap<StatusesForUpdateDto, Statuses>();

            
            CreateMap<Stops, StopsDto>();
            CreateMap<StopsForCreationDto, Stops>();
            CreateMap<StopsForUpdateDto, Stops>();
        }

    }
}
