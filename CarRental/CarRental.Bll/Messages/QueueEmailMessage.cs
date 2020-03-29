using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Messages
{
    public class QueueEmailMessage
    {
        public string To { get; set; }

        public string From { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public QueueEmailMessage()
        {
        }

        public QueueEmailMessage(string to, string from, string body, string subject)
        {
            To = to;
            From = from;
            Body = body;
            Subject = subject;
        }
    }
}
