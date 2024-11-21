using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.DAL.models.enums;

namespace FlightManagement.models
{
    [Table("Statuses")]
    public class Status
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        [NotMapped]
        public FlightStatus StatusEnum
        {
            get => Enum.TryParse(StatusName, out FlightStatus status) ? status : throw new ArgumentException("Invalid status");
            set => StatusName = value.ToString(); // Сохраняйте как строку
        }

    }
}
