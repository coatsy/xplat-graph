using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PropertyManager.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public Uri Endpoint { get; set; }

        public string AccessToken
        {
            get { return _httpClient.DefaultRequestHeaders.Authorization?.Parameter; }
            set
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", value);
            }
        }

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> GetAsync<T>(string resource)
        {
            if (AccessToken == null)
            {
                throw new Exception(
                    "The AccessToken is missing and needs " +
                    "to be set before using the HttpService.");
            }

            // Get the response.
            var response = await _httpClient.GetStringAsync(
                new Uri(Endpoint.AbsoluteUri + resource));

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        public async Task<T> PutAsync<T>(string resource, Stream stream, string contentType)
        {
            if (AccessToken == null)
            {
                throw new Exception(
                    "The AccessToken is missing and needs " +
                    "to be set before using the HttpService.");
            }

            // Create content.
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Get the response.
            var response = await _httpClient.PutAsync(
                new Uri(Endpoint.AbsoluteUri + resource), streamContent);

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync());
            return result;
        }
    }
}
