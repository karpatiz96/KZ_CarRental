using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CarRental.Functions
{
    public static class EmailMessageQueueTrigger
    {
        [FunctionName("EmailMessageQueueTrigger")]
        public static void Run([QueueTrigger("carrentalwebkzmailqueue", Connection = "AzureWebJobsStorage")]string email, ILogger log,
            [SendGrid(ApiKey = "SendGridKey")] out SendGridMessage message)
        {
            var queueEmail = JsonConvert.DeserializeObject<QueueEmailMessage>(email);

            message = new SendGridMessage()
            {
                From = new EmailAddress("", "CarRental Team"),
                Subject = queueEmail.Subject,
                PlainTextContent = queueEmail.Body,
                HtmlContent = queueEmail.Body
            };

            message.AddTo(queueEmail.To);
        }
    }

    public class QueueEmailMessage
    {
        public string To { get; set; }

        public string From { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }
    }
}
