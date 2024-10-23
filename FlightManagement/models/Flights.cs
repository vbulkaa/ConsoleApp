using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.DAL.models.enums;

namespace FlightManagement.models
{
    [Table("Flights")]
    public class Flights
    {
        [Key]
        public int FlightID { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string AircraftType { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть положительной")]
        public decimal TicketPrice { get; set; }
        public virtual ICollection<Routes> Routes { get; set; } = new List<Routes>(); 


        [NotMapped]
        public AircraftType TypeEnum
        {
            get => Enum.TryParse(AircraftType, out AircraftType status) ? status : throw new ArgumentException("Invalid status");
            set => AircraftType = value.ToString(); // Сохраняйте как строку
        }

        
    }
}
