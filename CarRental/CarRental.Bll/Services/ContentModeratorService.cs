using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class ContentModeratorService : IContentModeratorService
    {
        private IConfiguration _configuration { get; set; }

        public ContentModeratorService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<string> ModerateText(string input)
        {
            string subscriptionKey = _configuration.GetValue<String>("ContentModeratorKey");
            string endpoint = _configuration.GetValue<String>("ModeratorEndpoint");

            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(subscriptionKey));
            client.Endpoint = endpoint;

            // Convert string to a byte[], then into a stream (for parameter in ScreenText()).
            byte[] textBytes = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(textBytes);

            string moderatedText = "";

            using (client)
            {
                var outtext = await client.TextModeration.ScreenTextAsync("text/html", stream, "eng", true, true, null, true);
                moderatedText = outtext.AutoCorrectedText;
            }

            return moderatedText; 
        }
    }
}
