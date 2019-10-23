using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class ButtonConfirmationDto
    {
        public ButtonConfirmationDto(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public string Text { get; set; }

        public string Url { get; set; }
    }
}
