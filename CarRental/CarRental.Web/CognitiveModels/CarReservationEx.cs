using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.CognitiveModels
{
    public partial class CarReservation
    {
        public string PickUp
        {
            get
            {
                return Entities.datetime?.FirstOrDefault()?.Expressions?.FirstOrDefault()?.Split(',')?[0]?.TrimStart('(');
            }
        }

        public string DropOff
        {
            get
            {
                var dates = Entities.datetime?.FirstOrDefault()?.Expressions?.FirstOrDefault()?.Split(',');
                if (dates != null)
                {
                    if (dates.Length > 1)
                    {
                        return dates[1].ToString();
                    }
                }

                return null;
            }
        }

        public string Model 
            => Entities.Model?.FirstOrDefault()?.FirstOrDefault();

        public string TimexRange
            => Entities.datetime?.FirstOrDefault()?.Expressions?.FirstOrDefault()?.ToString();
    }
}
