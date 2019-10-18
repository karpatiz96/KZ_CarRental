using CarRental.Dal.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class VehicleModelDetailsDto : VehicleModelDto
    {
        public ICollection<CarDto> Cars { get; set; }

        public int CarFound { get; set; }

        public float StarRating { get; set; }

        public int Reviewers { get; set; }
    }
}
