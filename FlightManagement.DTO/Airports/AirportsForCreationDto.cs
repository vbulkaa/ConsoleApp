using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DTO.Airport
{
    public class AirportsForCreationDto
    {
        public int AirportID { get; set; }
        [Required(ErrorMessage = "Название аэропорта обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Локация аэропорта обязательна для заполнения")]
        [StringLength(200, ErrorMessage = "Локация не должна превышать 200 символов")]
        public string Location { get; set; }
    }
}
