using CarRental.Bll.IServices;
using CarRental.Bll.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly IConfiguration _configuration;

        public CloudStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(QueueEmailMessage message)
        {
            var storageConnectionString = _configuration.GetValue<String>("StorageConnectionString");

            if (!String.IsNullOrEmpty(storageConnectionString)) {
                CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
                CloudQueueClient client = account.CreateCloudQueueClient();
                CloudQueue queue = client.GetQueueReference("carrentalwebkzmailqueue");

                await queue.CreateIfNotExistsAsync();

                var serializedMessage = JsonConvert.SerializeObject(message);
                var queueMessage = new CloudQueueMessage(serializedMessage);

                await queue.AddMessageAsync(queueMessage);
            }
        }
    }
}
