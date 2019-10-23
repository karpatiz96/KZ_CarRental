using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class EmailConfirmationDto
    {
        public EmailConfirmationDto(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
