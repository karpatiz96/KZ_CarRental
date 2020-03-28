using CarRental.Bll.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class BlobService : IBlobService
    {
        private IConfiguration _configuration { get; }

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadImageToStorageAsync(IFormFile file)
        {
            var storageConnectionString = _configuration.GetValue<String>("StorageConnectionString");
            if (!String.IsNullOrEmpty(storageConnectionString))
            {
                try
                {
                    CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
                    CloudBlobClient client = account.CreateCloudBlobClient();
                    CloudBlobContainer container = client.GetContainerReference("images");
                    CloudBlockBlob blob = container.GetBlockBlobReference(Path.GetFileName(file.FileName));
                    using(var stream = file.OpenReadStream())
                    {
                        await blob.UploadFromStreamAsync(stream);
                    }
                }
                catch (Exception e)
                {

                }
            }

        }

    }
}
