using FlightManagement.DAL.models.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.models
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Принимает значение перечислений и возвращает строку , которая соответствует атрибутуб если задан. Если отсутствуетб то возвращается строковое представление
        /// </summary>
        /// <param name="aircraftType"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? enumValue.ToString();
        }

        public static AircraftType SetAircraftType(string typeStr, bool isDisplayName)
        {
            if (isDisplayName)
            {
                switch (typeStr)
                {
                    case "AirbusA320":
                        return AircraftType.AirbusA320;
                    case "Boeing737":
                        return AircraftType.Boeing737;
                    case "Embraer175":
                        return AircraftType.Embraer175;
                    case "BombardierCRJ900":
                        return AircraftType.BombardierCRJ900;
                    case "Boeing787":
                        return AircraftType.Boeing787;
                    default:
                        throw new Exception("Such type of aircraft is not found");
                }
            }

            if (!Enum.TryParse(typeStr, true, out AircraftType aircraftType))
            {
                throw new Exception("Such type of aircraft is not found");
            }

            return aircraftType;
        }

        public static FlightStatus SetFlightStatus(string statusStr, bool isDisplayName)
        {
            if (isDisplayName)
            {
                switch (statusStr)
                {
                    case "Вылет":
                        return FlightStatus.Вылет;
                    case "Промежуточный":
                        return FlightStatus.Промежуточный;
                    case "Прилёт":
                        return FlightStatus.Прилёт;
                    default:
                        throw new Exception("Such flight status is not found");
                }
            }

            if (!Enum.TryParse(statusStr, true, out FlightStatus flightStatus))
            {
                throw new Exception("Such flight status is not found");
            }

            return flightStatus;
        }
    }
}
