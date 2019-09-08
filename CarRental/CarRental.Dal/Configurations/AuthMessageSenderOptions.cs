using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Configurations
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
